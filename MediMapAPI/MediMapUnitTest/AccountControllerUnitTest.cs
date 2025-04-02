using DataAccess.Repository.IGenericRepository;
using DataAccess.Repository.iUnitOfWork;
using MediMapAPI.Controllers;
using MediMapAPI.Service;
using MediMapAPI.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Model.Dto;
using Models.ViewModel;
using Moq;
using System.Linq.Expressions;
using static MediMapAPI.Controllers.AccountController;
using Validator = MediMapAPI.Service.Validator;

namespace MediMapUnitTest;

[TestClass]
public class AccountControllerUnitTest
{

    private readonly Mock<IAuthService> _authService;
    private readonly Mock<UserManager<ApplicationUser>> _userManager;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly AccountController _accountController;
    private readonly Mock<SignInManager<ApplicationUser>> _mockSignInManager;
    private readonly Mock<IGenericRepository<ApplicationUser>> _mockUserRepository;
    private readonly Mock<ITokenService> _mockITokenService;
    private readonly AuthService _mockAuthService;


    public AccountControllerUnitTest()
    {
        var store = new Mock<IUserStore<ApplicationUser>>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
        _mockSignInManager = new Mock<SignInManager<ApplicationUser>>(_userManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>().Object, null, null, null, null);
        _mockITokenService = new Mock<ITokenService>();
        _authService = new Mock<IAuthService>();
        _mockUserRepository = new Mock<IGenericRepository<ApplicationUser>>();
        _unitOfWork.Setup(x => x.ApplicationUserRepository).Returns(_mockUserRepository.Object);
        _mockAuthService = new AuthService(_userManager.Object, _mockSignInManager.Object, _mockITokenService.Object);
        _accountController = new AccountController(_authService.Object, _userManager.Object, _unitOfWork.Object);
    }

    [TestMethod]
    public async Task CreateToken_ReturnsOk()
    {
        // Arrange
        var user = new UserAuthenication
        {
            Username = "EnesTekinbas51",
            Password = "EnesTekinbas51."
        };
        var response = new RefreshTokenResponse("kjfregijregjreigjirefjvreiusgtir9eqfug8efij", "9jiogtu39rwefopdutgriefwjurewk", 1, 1);
        _authService.Setup(x => x.AuthenticateUserAsync(user.Username, user.Password)).ReturnsAsync(response);

        // Act
        var result = await _accountController.CreateToken(user);
        // Assert

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, out OkObjectResult instance);
        Assert.AreEqual(200, instance.StatusCode);
    }
    [TestMethod]
    public async Task CreateToken_ReturnsUnauthorized()
    {
        // Arrange
        var user = new UserAuthenication
        {
            Username = "EnesTekinbas51",
            Password = "EnesTekinbas51."
        };
        _authService.Setup(x => x.AuthenticateUserAsync(user.Username, user.Password)).ReturnsAsync((RefreshTokenResponse)null);

        // Act
        var result = await _accountController.CreateToken(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, out UnauthorizedObjectResult instance);
        Assert.AreEqual(401, instance.StatusCode);

        var Djson = Deserializer.Deserialize(instance.Value);

        Assert.AreEqual("Gebruikersnaam of Wachtwoord is incorrect.", Djson.message);
    }
    [TestMethod]
    public async Task CreateToken_ReturnsStateCode()
    {
        // Arrange
        var user = new UserAuthenication
        {
            Username = "EnesTekinbas51",
            Password = "EnesTekinbas51."
        };
        _authService.Setup(x => x.AuthenticateUserAsync(user.Username, user.Password)).ThrowsAsync(new Exception());

        // Act
        var result = await _accountController.CreateToken(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, out ObjectResult instance);
        Assert.AreEqual(500, instance.StatusCode);

        var Djson = Deserializer.Deserialize(instance.Value);

        Assert.AreEqual("Er is een fout opgetreden bij het genereren van het token.", Djson.message);

    }
    [TestMethod]
    [DataRow(null, "fdgfg", "Gebruikersnaam mag niet leeg zijn.")]
    [DataRow("dgfdg", null, "Wachtwoord mag niet leeg zijn.")]
    public async Task CreateToken_IfValidationFailsReturnBadRequest(string userName, string password, string error)
    {

        // Arrange
        var user = new UserAuthenication
        {
            Username = userName,
            Password = password
        };

        // Act
        var result = await _accountController.CreateToken(user);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result.Result, out BadRequestObjectResult instance);
        Assert.AreEqual(400, instance.StatusCode);
        var Djson = Deserializer.Deserialize(instance.Value);
        Assert.AreEqual(error, Djson.message);
    }
    [TestMethod]
    [DataRow(null, "GoedWachtwoord51.", "enes@hotmail.com", "Gebruikersnaam mag niet leeg zijn.")]
    [DataRow("", "GoedWachtwoord51.", "enes@hotmail.com", "Gebruikersnaam mag niet leeg zijn.")]
    [DataRow("  ", "GoedWachtwoord51.", "enes@hotmail.com", "Gebruikersnaam mag niet leeg zijn.")]
    [DataRow("EnesTekinbas51", "GoedWachtwoord51.", "test@hotmail.com", "Gebruikersnaam mag alleen letters en cijfers bevatten.")]
    [DataRow("EnesTekinbas51", null, "enes@hotmail.com", "Wachtwoord mag niet leeg zijn.")]
    [DataRow("EnesTekinbas51", "", "enes@hotmail.com", "Wachtwoord mag niet leeg zijn.")]
    [DataRow("EnesTekinbas51", "Kort51.", "enes@hotmail.com", "Wachtwoord moet minimaal 10 tekens lang zijn.")]
    [DataRow("EnesTekinbas51", "FOUTWACHTWOORD!", "enes@hotmailcom", "Wachtwoord moet minimaal één kleine letter bevatten.")]
    [DataRow("EnesTekinbas51", "foutwachtwoord51.", "enes@hotmailcom", "Wachtwoord moet minimaal één hoofdletter bevatten.")]
    [DataRow("EnesTekinbas51", "FoutWachtwoord!", "enes@hotmailcom" , "Wachtwoord moet minimaal één cijfer bevatten.")]
    [DataRow("EnesTekinbas51", "FoutWachtwoord51", "enes@hotmail.com", "Wachtwoord moet minimaal één speciaal teken bevatten.")]
    [DataRow("EnesTekinbas51", "GoedWachtwoord51.", null, "Ongeldig e-mailformaat.")]
    [DataRow("EnesTekinbas51", "GoedWachtwoord51.", "", "Ongeldig e-mailformaat.")]
    [DataRow("EnesTekinbas51", "GoedWachtwoord51.", "test", "Ongeldig e-mailformaat.")]
    [DataRow("EnesTekinbas51", "GoedWachtwoord51.", "Enes@hotmail", "Ongeldig e-mailformaat.")]
    public void CreateAccount_ReturnBadRequestIfValidationFails(string username, string password, string email, string error)
    {
        // Arrange
        var validate = Validator.ValidateUserCredentials(username, password, email);
        // Act

        // Assert
        if (validate != null)
        {
            Assert.IsNotNull(validate);
            Assert.AreEqual(validate, error);
        }
    }
    //[TestMethod]
    //public async Task CreateAcoount_IfValid_ReturnOk()
    //{
    //    // Arrange

    //    var registerViewModel = new RegisterViewModel
    //    {
    //        CreateUserDto = new UserDto
    //        {
    //            Username = "EnesTekinbas51",
    //            Password = "EnesTekinbas51.",
    //            Email = "enes@hotmail.com",
    //            Id = 1
    //        },
    //        Arts = new ArtsDto
    //        {
    //            Id = 1,
    //            Specialisatie = "",
    //            Naam = "Artsnaam"
    //        },
    //        PatientDto = new PatientDto
    //        {
    //            Id = 0,
    //            VoorNaam = "testnaam",
    //            AchterNaam = "testnaam",
    //            AvatarNaam = "",
    //            AfspraakDatum = DateTime.Now,
    //            GeboorteDatum = DateTime.Now,
    //            ArtsNaam = "",
    //            OuderVoogdNaam = "",
    //            TrajectNaam = ""
    //        },
    //        OuderVoogd = new OuderVoogdDto
    //        {
    //            Id = 1,
    //            AchterNaam = "testnaam",
    //            VoorNaam = "Testnaam"
    //        },
    //        TrajectId = 1,
    //    };

    //    _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>())).ReturnsAsync(false);
    //    _userManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);
    //    _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()));

    //    // Act
    //    var result = await _accountController.CreateAccount(registerViewModel);

    //    Assert.IsNotNull(result);
    //    Assert.IsInstanceOfType(result, out OkObjectResult instance);
    //    Assert.AreEqual(200, instance.StatusCode);
    //    var Djson = Deserializer.Deserialize(instance.Value);
    //    Assert.AreEqual("User created successfully.", Djson.message);
    //}
    [TestMethod]
    public async Task createAccount_ReturnsConflict()
    {
        var registerViewModel = new RegisterViewModel
        {
            CreateUserDto = new UserDto
            {
                Username = "EnesTekinbas51",
                Password = "EnesTekinbas51.",
                Email = "enes@hotmail.com",
                Id = 1
            },
            Arts = new ArtsDto
            {
                Id = 1,
                Specialisatie = "",
                Naam = "Artsnaam"
            },
            PatientDto = new PatientDto
            {
                Id = 0,
                VoorNaam = "testnaam",
                AchterNaam = "testnaam",
                AvatarNaam = "",
                AfspraakDatum = DateTime.Now,
                GeboorteDatum = DateTime.Now,
                ArtsNaam = "",
                OuderVoogdNaam = "",
                TrajectNaam = ""
            },
            OuderVoogd = new OuderVoogdDto
            {
                Id = 1,
                AchterNaam = "testnaam",
                VoorNaam = "Testnaam"
            },
            TrajectId = 1,
        };
        _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>())).ReturnsAsync(true);

        // Act
        var result = await _accountController.CreateAccount(registerViewModel);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, out ConflictObjectResult instance);
        Assert.AreEqual(409, instance.StatusCode);
        var Djson = Deserializer.Deserialize(instance.Value);
        Assert.AreEqual("Gebruikersnaam bestaat al.", Djson.message);
    }
    [TestMethod]
    public async Task CreateAccount_EmailExist_ReturnBadRequest()
    {
        var registerViewModel = new RegisterViewModel
        {
            CreateUserDto = new UserDto
            {
                Username = "EnesTekinbas51",
                Password = "EnesTekinbas51.",
                Email = "enes@hotmail.com",
                Id = 1
            },
            Arts = new ArtsDto
            {
                Id = 1,
                Specialisatie = "",
                Naam = "Artsnaam"
            },
            PatientDto = new PatientDto
            {
                Id = 0,
                VoorNaam = "testnaam",
                AchterNaam = "testnaam",
                AvatarNaam = "",
                AfspraakDatum = DateTime.Now,
                GeboorteDatum = DateTime.Now,
                ArtsNaam = "",
                OuderVoogdNaam = "",
                TrajectNaam = ""
            },
            OuderVoogd = new OuderVoogdDto
            {
                Id = 1,
                AchterNaam = "testnaam",
                VoorNaam = "Testnaam"
            },
            TrajectId = 1,
        };
        _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>())).ReturnsAsync(false);

        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
        // Act
        var result = await _accountController.CreateAccount(registerViewModel);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, out BadRequestObjectResult instance);
        Assert.AreEqual(400, instance.StatusCode);
        var Djson = Deserializer.Deserialize(instance.Value);
        Assert.AreEqual("Email is al in gebruik.", Djson.message);
    }
    [TestMethod]
    public async Task CreateAccount_Exception_CheckUserExist_ReturnStatusCode()
    {
        var registerViewModel = new RegisterViewModel
        {
            CreateUserDto = new UserDto
            {
                Username = "EnesTekinbas51",
                Password = "EnesTekinbas51.",
                Email = "enes@hotmail.com",
                Id = 1
            },
            Arts = new ArtsDto
            {
                Id = 1,
                Specialisatie = "",
                Naam = "Artsnaam"
            },
            PatientDto = new PatientDto
            {
                Id = 0,
                VoorNaam = "testnaam",
                AchterNaam = "testnaam",
                AvatarNaam = "",
                AfspraakDatum = DateTime.Now,
                GeboorteDatum = DateTime.Now,
                ArtsNaam = "",
                OuderVoogdNaam = "",
                TrajectNaam = ""
            },
            OuderVoogd = new OuderVoogdDto
            {
                Id = 1,
                AchterNaam = "testnaam",
                VoorNaam = "Testnaam"
            },
            TrajectId = 1,
        };
        _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>())).ThrowsAsync(new Exception());

        // Act
        var result = await _accountController.CreateAccount(registerViewModel);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, out ObjectResult instance);
        Assert.AreEqual(500, instance.StatusCode);
        var Djson = Deserializer.Deserialize(instance.Value);
        Assert.AreEqual("Er is een fout opgetreden bij het aanmaken van de gebruiker.", Djson.message);
    }
    [TestMethod]
    public async Task CreateAccount_Exception_FindEmail_ReturnStatusCode()
    {
        var registerViewModel = new RegisterViewModel
        {
            CreateUserDto = new UserDto
            {
                Username = "EnesTekinbas51",
                Password = "EnesTekinbas51.",
                Email = "enes@hotmail.com",
                Id = 1
            },
            Arts = new ArtsDto
            {
                Id = 1,
                Specialisatie = "",
                Naam = "Artsnaam"
            },
            PatientDto = new PatientDto
            {
                Id = 0,
                VoorNaam = "testnaam",
                AchterNaam = "testnaam",
                AvatarNaam = "",
                AfspraakDatum = DateTime.Now,
                GeboorteDatum = DateTime.Now,
                ArtsNaam = "",
                OuderVoogdNaam = "",
                TrajectNaam = ""
            },
            OuderVoogd = new OuderVoogdDto
            {
                Id = 1,
                AchterNaam = "testnaam",
                VoorNaam = "Testnaam"
            },
            TrajectId = 1,
        };
        _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>())).ThrowsAsync(new Exception());
        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ThrowsAsync(new Exception());


        // Act
        var result = await _accountController.CreateAccount(registerViewModel);

        Assert.IsNotNull(result);
        Assert.IsInstanceOfType(result, out ObjectResult instance);
        Assert.AreEqual(500, instance.StatusCode);
        var Djson = Deserializer.Deserialize(instance.Value);
        Assert.AreEqual("Er is een fout opgetreden bij het aanmaken van de gebruiker.", Djson.message);
    }
    [TestMethod]
    public async Task RefreshToken_InvalidToken_ReturnsBadRequest()
    {
        // Arrange
        var request = new RefreshTokenRequest { RefreshToken = "invalid_token" };

        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync((ApplicationUser)null);


        // Act
        var result = await _accountController.RefreshToken(request);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result.Result;
        Assert.AreEqual("Invalid refresh token.", badRequestResult.Value);
    }

    [TestMethod]
    public async Task RefreshToken_ExpiredToken_ReturnsBadRequest()
    {
        // Arrange
        var request = new RefreshTokenRequest { RefreshToken = "expired_token" };
        var user = new ApplicationUser
        {
            RefreshToken = "expired_token",
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(-1) 
        };

        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(user);


        // Act
        var result = await _accountController.RefreshToken(request);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result.Result;
        Assert.AreEqual("Invalid refresh token.", badRequestResult.Value);
    }

    [TestMethod]
    public async Task RefreshToken_ValidToken_ReturnsNewTokens()
    {
        // Arrange
        var request = new RefreshTokenRequest { RefreshToken = "valid_token" };
        var user = new ApplicationUser
        {
            UserName = "testuser",
            RefreshToken = "valid_token",
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(1)
        };

        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(user);



        var expectedResponse = new RefreshTokenResponse("dfjdjfdf", "abcdefghiyklm", 1, 1);
  

        _authService.Setup(x => x.RefreshUserTokenAsync(user.UserName))
                    .ReturnsAsync(expectedResponse);

        // Act
        var result = await _accountController.RefreshToken(request);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result.Result;
        var response = (RefreshTokenResponse)okResult.Value;
        Assert.AreEqual(expectedResponse.Token, response.Token);
        Assert.AreEqual(expectedResponse.RefreshToken, response.RefreshToken);
    }

    [TestMethod]
    public async Task RefreshToken_NullResponseFromAuthService_ReturnsBadRequest()
    {
        // Arrange
        var request = new RefreshTokenRequest { RefreshToken = "valid_token" };
        var user = new ApplicationUser
        {
            UserName = "testuser",
            RefreshToken = "valid_token",
            RefreshTokenExpiry = DateTime.UtcNow.AddDays(1)
        };

        _mockUserRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(user);


        _authService.Setup(x => x.RefreshUserTokenAsync(user.UserName))
                       .ReturnsAsync((RefreshTokenResponse)null);

        // Act
        var result = await _accountController.RefreshToken(request);

        // Assert
        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
        var badRequestResult = (BadRequestObjectResult)result.Result;
        Assert.AreEqual("Invalid refresh token.", badRequestResult.Value);
    }


}
