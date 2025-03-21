using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediMapAPI.Service.Interface;
using MediMapAPI.Models;

namespace MediMapAPI.Service
{
    public class TokenService : ITokenService
    {
        private readonly string _jwtKey;
        private readonly string _audience;
        private readonly string _issuer;
        public TokenService(IOptions<TokenSettings> tokenSettings)
        {
            _jwtKey = tokenSettings.Value.Key ?? throw new ArgumentNullException("JWT Key is missing.");
            _audience = tokenSettings.Value.Audience ?? throw new ArgumentNullException("JWT Audience is missing.");
            _issuer = tokenSettings.Value.Issuer ?? throw new ArgumentNullException("JWT Issuer is missing.");
        }

        public string GenerateToken(IEnumerable<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Audience = _audience, // Set the audience
                Issuer = _issuer, // Set the issuer
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
