using MediMapAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Model.Dto;

namespace MediMapAPI.Controllers
{
    [Route("Account/[controller]")] // Use a consistent route prefix
    [ApiController] // Mark the controller as an API controller
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IAuthService authService,
                              UserManager<ApplicationUser> userManager)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> CreateToken(UserAuthenication user)
        {
            try
            {
                // Validate input
                var validationError = ValidateCredentials(user.Username, user.Password);
                if (validationError != null)
                {
                    return BadRequest(new { message = validationError });
                }

                // Authenticate the user
                var response = await _authService.AuthenticateUserAsync(user.Username, user.Password);
                if (response == null)
                {
                    return Unauthorized(new { message = "Gebruikersnaam of Wachtwoord is incorrect." });
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het genereren van het token." });
            }
        }
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null || user.RefreshTokenExpiry <= DateTime.UtcNow)
            {
                return BadRequest("Invalid refresh token.");
            }

            var response = await _authService.RefreshUserTokenAsync(user.UserName); // Await the result
            if (response == null)
            {
                return BadRequest("Invalid refresh token.");
            }

            return Ok(response);
        }
        // Helper method to validate credentials
        private static string ValidateCredentials(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "Username cannot be empty.";

            if (string.IsNullOrWhiteSpace(password))
                return "Password cannot be empty.";

            return null; // No errors
        }
        public class UserAuthenication
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
    
}
