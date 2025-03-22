using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MediMapAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProfileInformationController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public ProfileInformationController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // GET: api/<ProfileInformationController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<ProfileInformationController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ProfileInformationController>
    [HttpPost]
    public void Post([FromBody]string value)
    {

    }

    // PUT api/<ProfileInformationController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {

    }

    // DELETE api/<ProfileInformationController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
