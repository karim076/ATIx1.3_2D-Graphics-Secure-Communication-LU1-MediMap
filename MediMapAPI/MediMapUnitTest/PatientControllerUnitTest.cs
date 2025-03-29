using DataAccess.Repository.IGenericRepository;
using DataAccess.Repository.iUnitOfWork;
using MediMapAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Dto;
using Moq;
using System.Linq.Expressions;
using static MediMapAPI.Controllers.PatientController;

namespace MediMapUnitTest;

[TestClass]
public class PatientControllerUnitTest
{

    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<Patient>> _mockPatientRepository;
    private readonly PatientController _patientController;

    public PatientControllerUnitTest()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockPatientRepository = new Mock<IGenericRepository<Patient>>();
        _mockUnitOfWork.Setup(u => u.PatientRepository).Returns(_mockPatientRepository.Object);
        _patientController = new PatientController(_mockUnitOfWork.Object);
    }

    // voor de get methode
    [TestMethod]
    public async Task Get_ReturnsPatient()
    {
        //arrange
        var patientId = 1;
        var patient = new Patient
        {
            Id = patientId,
            VoorNaam = "test1",
            AchterNaam = "Test2wew",

            OuderVoogd = new OuderVoogd { Id = 1, VoorNaam = "test1", AchterNaam = "Test2wew" },
            Traject = new Traject { Id = 1, Naam = "test1" },
            Arts = new Arts { Id = 1, Naam = "test1" }
        };

        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(patient);

        //act
        var result = await _patientController.Get(patientId);

        //assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOfType(okResult.Value, typeof(PatientDto));
        Assert.AreEqual(200, okResult.StatusCode);
    }
    [TestMethod]
    public async Task Get_ReturnsNotFound()
    {
        //arrange
        var patientId = 1;
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Patient)null);
        //act
        var result = await _patientController.Get(patientId);
        //assert
        Assert.IsNotNull(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
    [TestMethod]
    public async Task Get_Exception_ReturnsBadRequest()
    {
        //arrange
        var patientId = 1;
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());
        //act
        var result = await _patientController.Get(patientId);
        //assert
        Assert.IsNotNull(result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
    // voor de put methode
    [TestMethod]
    public async Task Put_ReturnsOkAndPatientDto()
    {
        var patientId = 1;
        var patientDto = new PatientDto { Id = patientId, VoorNaam = "test1", AchterNaam = "Testw" };
        var patient = new Patient
        {
            Id = patientId,
            VoorNaam = "test1",
            AchterNaam = "Test2wew",

            Arts = new Arts { Id = 1, Naam = "test1" },
            Traject = new Traject { Id = 1, Naam = "test1" },
            OuderVoogd = new OuderVoogd { Id = 1, VoorNaam = "test1", AchterNaam = "Test5656" }
        };
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(patient);

        //act
        var result = await _patientController.Put(patientId, patientDto);

        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.IsInstanceOfType(okResult.Value, typeof(PatientDto));
        Assert.AreEqual(200, okResult.StatusCode);

        var returnedDto = okResult.Value as PatientDto;
        Assert.IsNotNull(returnedDto);
        Assert.AreEqual(patientDto.VoorNaam, returnedDto.VoorNaam);
        Assert.AreEqual(patientDto.AchterNaam, returnedDto.AchterNaam);
    }
    [TestMethod]
    public async Task Put_ReturnsNotFound()
    {
        var patientId = 1;
        var patientDto = new PatientDto { Id = patientId, VoorNaam = "test1", AchterNaam = "Test2wew" };

        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync((Patient)null);
        //act
        var result = await _patientController.Put(patientId, null);
        Assert.IsNotNull(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
    [TestMethod]
    public async Task Put_Exception_ReturnsBadRequest()
    {
        var patientId = 1;
        var patientDto = new PatientDto { Id = patientId, VoorNaam = "test1", AchterNaam = "test1" };
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());
        //act
        var result = await _patientController.Put(patientId, patientDto);
        Assert.IsNotNull(result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
    [TestMethod]
    public async Task Put_ModelState_NotValid()
    {
        //arrange
        var patientId = 1;
        var patientDto = new PatientDto { Id = patientId, VoorNaam = string.Empty, AchterNaam = string.Empty };

        _patientController.ModelState.AddModelError("VoorNaam", "VoorNaam is verplicht");
        _patientController.ModelState.AddModelError("AchterNaam", "AchterNaam is verplicht");

        //act
        var result = await _patientController.Put(patientId, patientDto);

        //assert
        Assert.IsNotNull(result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
    [TestMethod]
    public async Task UpdateAvatar_ReturnsOk()
    {
        //arrange
        var patientId = 1;
        var avatarName = new AvatarName { Avatar = "test1" };
        var patient = new Patient { Id = patientId, VoorNaam = "test1", AchterNaam = "Test2wew" };

        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(patient);

        //act
        var result = await _patientController.UpdateAvatar(patientId, avatarName);
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }
    [TestMethod]
    public async Task UpdateAvatar_NotFound()
    {
        //arrange
        var patientId = 1;
        Patient patient = null!;
        var avatarName = new AvatarName { Avatar = "test1" };
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(patient);

        //act
        var result = await _patientController.UpdateAvatar(patientId, avatarName);

        //assert
        Assert.IsNotNull(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
    [TestMethod]
    public async Task UpdateAvatar_Exception_RetunsBadRequest()
    {
        //arrange
        var patientId = 1;
        var avatarName = new AvatarName { Avatar = "test1" };
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());
        //act
        var result = await _patientController.UpdateAvatar(patientId, avatarName);

        //assert
        Assert.IsNotNull(result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
    [TestMethod]
    public async Task GetAvatar_ReturnOk()
    {
        //arrange
        var patientId = 1;
        var patient = new Patient { Id = patientId, AvatarNaam = "Monster1" };
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(patient);
        //act
        var result = await _patientController.GetAvatar(patientId);
        //assert
        Assert.IsNotNull(result);
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }
    [TestMethod]
    public async Task GetAvatar_NotFound()
    {
        //arrange
        var patientId = 1;
        Patient patient = null!;
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ReturnsAsync(patient);
        //act
        var result = await _patientController.GetAvatar(patientId);
        //assert
        Assert.IsNotNull(result);
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
    [TestMethod]
    public async Task GetAvatar_Exception_ReturnsBadRequest()
    {
        //arrange
        var patientId = 1;
        _mockPatientRepository.Setup(p => p.GetAsync(It.IsAny<Expression<Func<Patient, bool>>>(), It.IsAny<string>()))
            .ThrowsAsync(new Exception());
        //act
        var result = await _patientController.GetAvatar(patientId);
        //assert
        Assert.IsNotNull(result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
    [TestMethod]
    public async Task GetAvatar_ModelState_NotValid()
    {
        //arrange
        AvatarName avatarName = null!;
        var patientId = 0;
        _patientController.ModelState.AddModelError("Id", "Id is verplicht");
        //act
        var result = await _patientController.GetAvatar(patientId);
        //assert
        Assert.IsNotNull(result);
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
}
