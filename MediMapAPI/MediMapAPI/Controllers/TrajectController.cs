using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediMapAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]

public class TrajectController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TrajectController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    
    [HttpGet("{id}")]
    public async Task<ActionResult<string>> GetTrajectByIdAsync(int id)
    {
        //if (!ModelState.IsValid)
        //{
        //    return BadRequest(ModelState);
        //}

        //try
        //{
        //    //var traject = await _unitOfWork.
        //}
        return "value";
    }

    [HttpPost]
    public void Post([FromBody] string value)
    {

    }

    
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {

    }

    
    [HttpDelete("{id}")]
    public void Delete(int id)
    {

    }
}

