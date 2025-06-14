using Contracts.IServices;
using Microsoft.AspNetCore.Mvc;
using MirraApi.Models.RequestModels;
using MirraApi.Models.Responses;

namespace Mirra.API.Controllers;

[ApiController]
[Route("auth")]
public class AuthController(IAuthenticationService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserForAuthentication loginDto)
    {
        var result = await authService.ValidateUser(loginDto);
        if (!result.Success)
        {
            return Unauthorized(new { message = ((BadUserBadRequestResponse)result).Message  });
        }

        var token = await authService.CreateToken(populateExp: true);
        
        return Ok(new { token = token.AccessToken, refreshToken = token.RefreshToken });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] Token refreshTokenDto)
    {
        var result = await authService.RefreshToken(refreshTokenDto);
        if (!result.Success)
        {
            return Unauthorized(new { message = ((RefreshTokenBadRequestResponse)result).Message });
        }

        var token = ((ApiOkResponse<Token>)result).Result;
        
        return Ok(new { token = token.AccessToken, refreshToken = token.RefreshToken });
    }
} 