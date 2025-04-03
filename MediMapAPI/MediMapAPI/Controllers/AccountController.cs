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
using Models.ViewModel;
using Models.Model;

namespace MediMapAPI.Controllers
{
    [Route("/[controller]")] // Use a consistent route prefix
    [ApiController] // Mark the controller as an API controller
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IAuthService authService,
                              UserManager<ApplicationUser> userManager,
                              IUnitOfWork unitOfWork)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
        public async Task<ActionResult> CreateAccount(RegisterViewModel user)
        {
            try
            {

                if (user.CreateUserDto == null || user.CreateUserDto.Password == null || user.CreateUserDto.Username == null || user.CreateUserDto.Email == null)
                {
                    return BadRequest(new { message = "Gebruikersnaam en wachtwoord zijn verplicht." });
                }
                if (user.PatientDto == null)
                {
                    return BadRequest(new { message = "Patient gegevens zijn verplicht." });
                }
                if (user.Arts == null)
                {
                    return BadRequest(new { message = "Arts gegevens zijn verplicht." });
                }
                if (user.OuderVoogd == null)
                {
                    return BadRequest(new { message = "OuderVoogd gegevens zijn verplicht." });
                }

                // Validate input
                var validationError = Validator.ValidateUserCredentials(user.CreateUserDto.Username, user.CreateUserDto.Password, user.CreateUserDto.Email);
                if (validationError != null)
                {
                    return BadRequest(new { message = validationError });
                }

                // Check if the username already exists
                if (await _unitOfWork.ApplicationUserRepository.AnyAsync(u => u.UserName == user.CreateUserDto.Username))
                {
                    return Conflict(new { message = "Gebruikersnaam bestaat al." });
                }
                // Check if the email is already registered
                var existingUser = await _unitOfWork.ApplicationUserRepository.GetAsync(u => u.Email == user.CreateUserDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "Email is al in gebruik." });
                }

                // Create the new user
                var newUser = new ApplicationUser
                {
                    UserName = user.CreateUserDto.Username,
                    NormalizedUserName = user.CreateUserDto.Username.ToUpper(),
                    Email = user.CreateUserDto.Email,
                    NormalizedEmail = user.CreateUserDto.Email?.ToUpper(),
                    PasswordHash = SecureHash.Hash(user.CreateUserDto.Password), // Hash the password
                    RefreshToken = "", // Explicitly set to null = ""
                    RefreshTokenExpiry = null, // Explicitly set to NULL
                    PatienId = null // Tijdelijk
                };

                await _userManager.CreateAsync(newUser);

                if(newUser.Id == 0)
                {
                    return BadRequest(new { message = "Er is een fout opgetreden bij het aanmaken van de gebruiker." });
                }

                // pak de patient, ouderVoogd en arts uit de viewmodel

                var patient = DtoToModel(user.PatientDto);
                var ouderVoogd = DtoToModel(user.OuderVoogd);
                var arts = DtoToModel(user.Arts);

                // voeg de patient, ouderVoogd en arts toe aan de database
                await _unitOfWork.ArtsRepository.AddAsync(arts);
                await _unitOfWork.OuderVoogdRepository.AddAsync(ouderVoogd);
                await _unitOfWork.SaveAsync();

                // pak de id van oudervoogd en arts en voeg deze toe aan de patient
                patient.OuderVoogdId = ouderVoogd.Id;
                patient.ArtsId = arts.Id;

                // Get Traject By Id
                if (user.TrajectId != 0)
                {
                    var traject = await _unitOfWork.TrajectRepository.GetAsync(t => t.Id == user.TrajectId);

                    if (traject == null)
                    {
                        return BadRequest(new { message = "Traject bestaat niet." });
                    }
                    patient.TrajectId = user.TrajectId;
                }

                await _unitOfWork.PatientRepository.AddAsync(patient);
                await _unitOfWork.SaveAsync();

                newUser.PatienId = patient.Id;
                await _userManager.UpdateAsync(newUser);

                // Assign the "User" role by default
                await _userManager.AddToRoleAsync(newUser, "User");

                return Ok(new { message = "User created successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Er is een fout opgetreden bij het aanmaken van de gebruiker." });
            }
        }
        [HttpPost("RefreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult<RefreshTokenResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetAsync(u => u.RefreshToken == request.RefreshToken);
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

        private Patient DtoToModel(PatientDto patientDto)
        {
            return new Patient
            {
                Id = patientDto.Id,
                VoorNaam = patientDto.VoorNaam,
                AchterNaam = patientDto.AchterNaam,
                GeboorteDatum = patientDto.GeboorteDatum,
                AfspraakDatum = patientDto.AfspraakDatum,
                AvatarNaam = patientDto.AvatarNaam,
                OuderVoogdId = patientDto.OuderVoogdId,
                ArtsId = patientDto.ArtsId,
                TrajectId = patientDto.TrajectId,
                PathLocation = 0,
            };
        }
        private Arts DtoToModel(ArtsDto artsDto)
        {
            return new Arts
            {
                Id = artsDto.Id,
                Naam = artsDto.Naam,
                Specialisatie = artsDto.Specialisatie,
            };
        }
        private OuderVoogd DtoToModel(OuderVoogdDto ouderVoogdDto)
        {
            return new OuderVoogd
            {
                Id = ouderVoogdDto.Id,
                VoorNaam = ouderVoogdDto.VoorNaam,
                AchterNaam = ouderVoogdDto.AchterNaam,
            };
        }
    }
    
}
