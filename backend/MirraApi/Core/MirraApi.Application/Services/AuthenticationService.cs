using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Contracts.IRepositories;
using Contracts.IServices;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MirraApi.Domain.ConfigurationModels;
using MirraApi.Domain.Entities;
using MirraApi.Models.RequestModels;
using MirraApi.Models.Responses;

namespace MirraApi.Application.Services;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryManager _manager;
    private readonly IOptions<JwtConfiguration> _configuration;
    private readonly JwtConfiguration _jwtConfiguration;

    private User? _user;

    public AuthenticationService(
        IMapper mapper,
        IRepositoryManager manager,
        IOptions<JwtConfiguration> configuration)
    {
        _mapper = mapper;
        _manager = manager;
        _configuration = configuration;
        _jwtConfiguration = _configuration.Value;
    }
    
    public async Task<ApiBaseResponse> RegisterUser(UserForRegistration userForRegistration)
    {
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(userForRegistration.Password);
        
        var user = _mapper.Map<User>(userForRegistration);
        user.PasswordHash = passwordHash;
        try
        {
            _manager.User.CreateUser(user);
            await _manager.SaveAsync();
        }
        catch (Exception e)
        {
            return new InvalidEmailBadRequestResponse();
        }

        return new ApiOkResponse<User>(user);
    }
    
    public async Task<ApiBaseResponse> ValidateUser(UserForAuthentication userForAuth)
    {
        _user = await _manager.User.GetUserByEmailAsync(userForAuth.Email!);
        
        if (_user == null || !BCrypt.Net.BCrypt.Verify(userForAuth.Password, _user.PasswordHash))
        {
            return new BadUserBadRequestResponse();
        }
        
        return new ApiOkResponse<User>(_user);
    }
    
    public async Task<Token> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();

        _user!.RefreshToken = refreshToken;
        _user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

        _manager.User.UpdateUser(_user);
        await _manager.SaveAsync();

        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        return new Token(accessToken, refreshToken);
    }
    
    private SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtConfiguration.Secret!);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    private List<Claim> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, _user!.Email),
        };

        return claims;
    }
    
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken
        (
            issuer: _jwtConfiguration.ValidIssuer,
            audience: _jwtConfiguration.ValidAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtConfiguration.Expires)),
            signingCredentials: signingCredentials
        );

        return tokenOptions;
    }
    
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret!)),
            ValidateLifetime = false,
            ValidIssuer = _jwtConfiguration.ValidIssuer,
            ValidAudience = _jwtConfiguration.ValidAudience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null ||
            !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid token");
        }

        return principal;
    }
    
    public async Task<ApiBaseResponse> RefreshToken(Token tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);

        var userEmail = principal.FindFirst(ClaimTypes.Email)?.Value;
        var user = await _manager.User.GetUserByEmailAsync(userEmail);

        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return new RefreshTokenBadRequestResponse();
        }

        _user = user;

        var token = await CreateToken(populateExp: false);

        return new ApiOkResponse<Token>(token);
    }
}