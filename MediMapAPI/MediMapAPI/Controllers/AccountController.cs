using MediMapAPI.Models;
using MediMapAPI.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Model.Dto;
using MediMapAPI.Service;
using DataAccess.Repository.iUnitOfWork;
using MediMap.Repositories;

namespace MediMapAPI.Controllers
{
    [Route("/[controller]")] // Use a consistent route prefix
    [ApiController] // Mark the controller as an API controller
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly UserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IAuthService authService,
                              UserManager<ApplicationUser> userManager,
                              UserRepository userRepository,
                              IUnitOfWork unitOfWork)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult<string>> CreateToken(UserAuthenication user)
        {
            try
            {
                // Validate input
                var validationError = Validator.ValidateCredentials(user.Username, user.Password);
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
        // Create a new user
        [HttpPost("Create")]
        [AllowAnonymous]
        public async Task<ActionResult> CreateAccount([FromBody] CreateUserDto user)
        {
            try
            {
                // Validate input
                var validationError = Validator.ValidateUserCredentials(user.Username, user.Password, user.Email);
                if (validationError != null)
                {
                    return BadRequest(new { message = validationError });
                }

                // Check if the username already exists
                if (await _userRepository.UserExistsAsync(user.Username))
                {
                    return Conflict(new { message = "Username already exists." });
                }
                // Check if the email is already registered
                var existingUser = await _userManager.FindByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Email is already registered." });
                }

                // Create the new user
                var newUser = new ApplicationUser
                {
                    UserName = user.Username,
                    NormalizedUserName = user.Username.ToUpper(),
                    Email = user.Email,
                    NormalizedEmail = user.Email?.ToUpper(),
                    PasswordHash = SecureHash.Hash(user.Password), // Hash the password
                    RefreshToken = "", // Explicitly set to null = ""
                    RefreshTokenExpiry = null // Explicitly set to NULL
                };

                await _userManager.CreateAsync(newUser);

                // Assign the "User" role by default
                // await _userManager.AddToRoleAsync(newUser, "User");

                return Ok(new { message = "User created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the user." });
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
        
        public class UserAuthenication
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
    
}
