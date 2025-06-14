using MirraApi.Models.RequestModels;
using MirraApi.Models.Responses;

namespace Contracts.IServices;

public interface IAuthenticationService
{
    Task<ApiBaseResponse> RegisterUser(UserForRegistration userForRegistration);
    Task<ApiBaseResponse> ValidateUser(UserForAuthentication userForAuth);
    Task<Token> CreateToken(bool populateExp);
    Task<ApiBaseResponse> RefreshToken(Token token);
}