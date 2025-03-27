using MediMapAPI.Models;
using MediMapAPI.Service.Interface;
using Microsoft.AspNetCore.Identity;
using Models;
using Models.Model.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MediMapAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<RefreshTokenResponse> AuthenticateUserAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !SecureHash.Verify(password, user.PasswordHash))
            {
                return null; // Invalid credentials
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = _tokenService.GenerateToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();
            // validate token and refresh token
            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            // Save the refresh token to the database
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Set refresh token expiry
            await _userManager.UpdateAsync(user);
            // create refresh token response
            var response = new RefreshTokenResponse(token, refreshToken, user.Id, user.PatienId);
            return (response);
        }

        public async Task<RefreshTokenResponse> RefreshUserTokenAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            // Check if user exists
            if (user == null)
            {
                return null; // Invalid credentials
            }

            // Check if the refresh token is still valid
            if (user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return null; // Refresh token has expired
            }

            // Get user roles
            var userRoles = await _userManager.GetRolesAsync(user);

            // Generate claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            // Add roles to claims
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate a new access token
            var token = _tokenService.GenerateToken(claims);

            // Generate a new refresh token ONLY if the current one is about to expire
            string refreshToken = user.RefreshToken;
            if (user.RefreshTokenExpiry <= DateTime.UtcNow.AddDays(1)) // Refresh if expiry is within 1 day
            {
                refreshToken = _tokenService.GenerateRefreshToken();
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Set new expiry
                await _userManager.UpdateAsync(user);
            }

            // Create refresh token response
            var response = new RefreshTokenResponse(token, refreshToken, user.Id, user.PatienId);

            return response;
        }

        private class loginResponse{
            
        }
        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
