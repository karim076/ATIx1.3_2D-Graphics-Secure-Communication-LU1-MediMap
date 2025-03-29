using DataAccess.Repository.IGenericRepository;
using DataAccess.Repository.iUnitOfWork;
using MediMapAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Dto;
using Moq;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace MediMapUnitTest;

[TestClass]
public class TrajectControllerUnitTest
{

    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Mock<IGenericRepository<Traject>> _trajectRepository;
    private readonly TrajectController _controller;

    public TrajectControllerUnitTest()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _trajectRepository = new Mock<IGenericRepository<Traject>>();
        _unitOfWork.Setup(u => u.TrajectRepository).Returns(_trajectRepository.Object);
        _controller = new TrajectController(_unitOfWork.Object);
    }
    [TestMethod]
    public async Task GetAllTrajects_ReturnsOk()
    {
        // Arrange
        IEnumerable<Traject> trajects = new List<Traject>
        {
            new Traject { Id = 1, Naam = "Traject 1" },
            new Traject { Id = 2, Naam = "Traject 2" }
        };

        _trajectRepository.Setup(r => r.GetAllAsync(It.IsAny<string>())).ReturnsAsync(trajects);

        // Act
        var result = await _controller.GetAllTrajects();

        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var valueResult = okResult.Value as IEnumerable<TrajectDto>;
        Assert.IsNotNull(valueResult);
        Assert.AreEqual(trajects.Count(), valueResult.Count());
    }
    [TestMethod]
    public async Task GetAllTrajects_ReturnsNotFound()
    {
        // Arrange
        IEnumerable<Traject> traject = null!;
        _trajectRepository.Setup(r => r.GetAllAsync(It.IsAny<string>())).ReturnsAsync(traject);
        // Act
        var result = await _controller.GetAllTrajects();
        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);

        var resultMessage = Deserialize(notFoundResult.Value);

        Assert.AreEqual("Geen traject gevonden.", resultMessage.message);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
    [TestMethod]
    public async Task GetAllTrajects_ModelState_NotValid()
    {
        // Arrange
        _controller.ModelState.AddModelError("key", "error message");

        // Act
        var result = await _controller.GetAllTrajects();
        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
    [TestMethod]
    public async Task GetAllTrajects_Exception_ReturnBadRequest()
    {
        // Arrange
        _trajectRepository.Setup(r => r.GetAllAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Fout fout fout"));
        // Act
        var result = await _controller.GetAllTrajects();
        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);

        var resultMessage = Deserialize(badRequestResult.Value);

        Assert.AreEqual("Fout fout fout", resultMessage.message);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
    [TestMethod]
    public async Task GetTrajectByIdAsync_ReturnsOk()
    {
        // Arrange
        var traject = new Traject { Id = 1, Naam = "UnitTesten is leuk" };
        _trajectRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Traject, bool>>>(), It.IsAny<string>())).ReturnsAsync(traject);
        // Act
        var result = await _controller.GetTrajectByIdAsync(1);
        // Assert
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var valueResult = okResult.Value as TrajectDto;
        Assert.IsNotNull(valueResult);
        Assert.AreEqual(traject.Id, valueResult.Id);
    }
    [TestMethod]
    public async Task GetTrajectByIdAsync_ReturnsNotFound()
    {
        // Arrange
        Traject traject = null!;
        _trajectRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Traject, bool>>>(), It.IsAny<string>())).ReturnsAsync(traject);
        // Act
        var result = await _controller.GetTrajectByIdAsync(1);
        // Assert
        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        var resultMessage = Deserialize(notFoundResult.Value);
        Assert.AreEqual("Geen traject gevonden.", resultMessage.message);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
    [TestMethod]
    public async Task GetTrajectByIdAsync_ModelState_NotValid()
    {
        // Arrange
        _controller.ModelState.AddModelError("key", "fout");
        // Act
        var result = await _controller.GetTrajectByIdAsync(1);
        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }
    [TestMethod]
    public async Task GetTrajectByIdAsync_Exception_ReturnsBadRequest()
    {
        // Arrange
        _trajectRepository.Setup(r => r.GetAsync(It.IsAny<Expression<Func<Traject, bool>>>(), It.IsAny<string>())).ThrowsAsync(new Exception("Fout fout fout"));
        // Act
        var result = await _controller.GetTrajectByIdAsync(1);
        // Assert
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        var resultMessage = Deserialize(badRequestResult.Value);
        Assert.AreEqual("Fout fout fout", resultMessage.message);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }


    private Error Deserialize(object value)
    {
        var sjson = JsonConvert.SerializeObject(value);
        var json = JsonConvert.DeserializeObject<Error>(sjson);
        return json;
    }
}
public class Error
{
    public string message = string.Empty;
}
