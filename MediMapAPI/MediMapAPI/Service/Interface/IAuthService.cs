using Models.Model.Dto;

namespace MediMapAPI.Service.Interface
{
    public interface IAuthService
    {
        Task<RefreshTokenResponse> AuthenticateUserAsync(string username, string password);
        Task<RefreshTokenResponse> RefreshUserTokenAsync(string username);
        Task SignOutAsync();
    }
}
