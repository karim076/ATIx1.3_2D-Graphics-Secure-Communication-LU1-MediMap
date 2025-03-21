using System.Security.Claims;

namespace MediMapAPI.Service.Interface
{
    public interface ITokenService
    {
        string GenerateToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
    }
}
