using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Dto;

namespace MediMapAPI.Controllers;


[Route("api/[controller]")]
[ApiController]

public class TrajectController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public TrajectController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet("all")]
    [AllowAnonymous]
    public async Task<ActionResult<TrajectDto>> GetAllTrajects()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var trajects = await _unitOfWork.TrajectRepository.GetAllAsync(includeProperty:"TrajectZorgMomenten");
            if (trajects == null)
            {
                return NotFound(new { message = "Geen traject gevonden." });
            }

            List<TrajectDto> trajectsDto = new List<TrajectDto>(); // Initialize the list
            foreach (Traject traject in trajects)
            {
                var trajectDto = TrajectDto(traject);
                if (trajectDto == null)
                {
                    return BadRequest(new { message = "Fout bij het ophalen van traject." });
                }
                else
                {
                    trajectsDto.Add(trajectDto);
                }
            }

            

            return Ok(trajectsDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    
    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<TrajectDto>> GetTrajectByIdAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try { 
            var traject = await _unitOfWork.TrajectRepository.GetAsync(t => t.Id == id);
            if (traject == null)
            {
                return NotFound(new { message = "Geen traject gevonden." });
            }
            var trajectDto = TrajectDto(traject);

            if (trajectDto == null)
            {
                return BadRequest(new { message = "Fout bij het ophalen van traject." });
            }

            return Ok(trajectDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
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

    private TrajectDto TrajectDto(Traject traject)
    {
        return new TrajectDto
        {
            Id = traject.Id,
            Naam = traject.Naam,
            //Patients = traject.Patients,
            TrajectZorgMomenten = traject.TrajectZorgMomenten
        };
    }
}

