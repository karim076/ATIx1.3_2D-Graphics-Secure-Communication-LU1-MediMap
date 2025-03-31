using DataAccess.Repository.IGenericRepository;
using DataAccess.Repository.iUnitOfWork;
using MediMapAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Model;
using Models.Model.Dto;
using Moq;
using System.Linq.Expressions;

namespace MediMapUnitTest;

[TestClass]
public class UserControllerUnitTest
{
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IGenericRepository<ApplicationUser>> _userRepository;
    private readonly Mock<IGenericRepository<Patient>> _patientRepository;
    private readonly UserController _userController;

    public UserControllerUnitTest()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _userRepository = new Mock<IGenericRepository<ApplicationUser>>();
        _patientRepository = new Mock<IGenericRepository<Patient>>();
        _unitOfWork.Setup(u => u.ApplicationUserRepository).Returns(_userRepository.Object);
        _unitOfWork.Setup(u => u.PatientRepository).Returns(_patientRepository.Object);
        _userController = new UserController(_unitOfWork.Object);
    }

    [TestMethod]
    public async Task GetUserByIdAsync_ValidId_ReturnsUser()
    {
        // Arrange
        int userId = 1; 
        var user = new ApplicationUser
        {
            Id = userId,
            UserName = "TestUser",
            PatienId = 1,
            Email = "Enes@hotmail.com",
            Patient = new Patient
            {
                Id = 1,
                ArtsId = 1,
                TrajectId = 1,
                PathLocation = 1,
                OuderVoogdId = 1,
                VoorNaam = "Test",
                AchterNaam = "User",
                GeboorteDatum = new DateTime(2000, 1, 1)
            }
        };

        _userRepository.Setup(u => u.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(user);

        // Act
        var result = await _userController.GetUserByIdAsync(userId);

        // Assert
        var okResult = result.Result as OkObjectResult;

        Assert.AreEqual(200, okResult.StatusCode);
        Assert.IsNotNull(okResult.Value);
    }
    [TestMethod]
    public async Task GetUserByIdAsync_InvalidId_ReturnsNotFound()
    {
        // Arrange
        int userId = 1343434;
        ApplicationUser user = null;
        _userRepository.Setup(u => u.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(user);
        // Act
        var result = await _userController.GetUserByIdAsync(userId);
        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.IsNotNull(notFoundResult.Value);
    }
    [TestMethod]
    public async Task Put_ValidUpdate_ReturnsOk()
    {
        //Arrange
        int userId = 1;
        var userdto = new CreateUserDto
        {
            TrajectId = 1,
            PatientPathLocation = 1
        };
        var applciationUser = new ApplicationUser
        {
            Id = userId,
            Patient = new Patient
            {
                Id = 1,
                TrajectId = 1,
                PathLocation = 1
            }
        };
        _userRepository.Setup(u => u.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(applciationUser);
        _patientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>())).ReturnsAsync(applciationUser.Patient);

        //Act   
        var result = await _userController.Put(userId, userdto);

        //Assert
        var okResult = result.Result as OkObjectResult;
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.IsNotNull(okResult.Value);

    }
    [TestMethod]
    public async Task Put_InvalidUpdate_ReturnsNotFound()
    {
        //Arrange
        int userId = 1;
        var userdto = new CreateUserDto
        {
            TrajectId = 1,
            PatientPathLocation = 1
        };
        ApplicationUser applciationUser = null;
        _userRepository.Setup(u => u.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(applciationUser);
        //Act   
        var result = await _userController.Put(userId, userdto);
        //Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.IsNotNull(notFoundResult.Value);
    }
    [TestMethod]
    public async Task Put_InvalidPatient_ReturnsNotFound()
    {
        //Arrange
        var userId = 1;
        var userdto = new CreateUserDto
        {
            TrajectId = 1,
            PatientPathLocation = 1
        };
        var applciationUser = new ApplicationUser
        {
            Id = userId,
            Patient = null
        };
        _userRepository.Setup(u => u.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).ReturnsAsync(applciationUser);
        //Act
        var result = await _userController.Put(userId, userdto);
        //Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.IsNotNull(notFoundResult.Value);

    }
    [TestMethod]
    public async Task Put_InvalidModelState_ReturnsBadRequest()
    {
        //Arrange
        _userController.ModelState.AddModelError("TrajectId", "Required");
        _userController.ModelState.AddModelError("PatientPathLocation", "Required");

        //Act
        var result = await _userController.Put(1, null);
        //Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.IsNotNull(badRequestResult.Value);
    }
    [TestMethod]
    public async Task Put_Exception_ReturnsBadRequest()
    {
        //Arrange
        int userId = 1;
        var userdto = new CreateUserDto
        {
            TrajectId = 1,
            PatientPathLocation = 1
        };
        _userRepository.Setup(u => u.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).Throws(new Exception());
        //Act
        var result = await _userController.Put(userId, userdto);
        //Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.IsNotNull(badRequestResult.Value);
    }
    [TestMethod]
    public async Task GetUserById_Exception_ReturnBadRequest()
    {
        //arrange
        int userId = 1;
        _userRepository.Setup(u => u.GetAsync(It.IsAny<Expression<Func<ApplicationUser, bool>>>(), It.IsAny<string>())).Throws(new Exception());

        //Act
        var result = await _userController.GetUserByIdAsync(userId);

        //Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.IsNotNull(badRequestResult.Value);
    }
}
