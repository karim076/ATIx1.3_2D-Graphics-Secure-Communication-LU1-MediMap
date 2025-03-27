using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Model;
using Models.Model.Dto;

namespace MediMapAPI.Controllers;


[Route("api/[controller]")]
[ApiController]

public class UserController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public UserController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<CreateUserDto>> GetUserByIdAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try {
            var user = await _unitOfWork.ApplicationUserRepository.GetAsync(p => p.Id == id, includeProperty: "Patient");
            if (user == null)
            {
                return NotFound(new { message = "Geen user gevonden." });
            }

            var traject = await _unitOfWork.TrajectRepository.GetAsync(t => t.Id == user.Patient.TrajectId);
            //user.Patient.Traject = traject;


            var userDto = UserDto(user);

            if (userDto == null)
            {
                return BadRequest(new { message = "Fout bij het ophalen van traject." });
            }

            return Ok(userDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

    //[HttpPost]
    //public void Post([FromBody] string value)
    //{

    //}


    [HttpPut("{id}")]
    public async Task<ActionResult<CreateUserDto>> Put(int id, CreateUserDto updateUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetAsync(p => p.Id == id);
            if (user == null)
            {
                return NotFound(new { message = "Geen user gevonden." });
            }

            var traject = await _unitOfWork.TrajectRepository.GetAsync(t => t.Id == user.Patient.TrajectId);
            //user.Patient.Traject = traject;


            var userDto = UserDto(user);

            if (userDto == null)
            {
                return BadRequest(new { message = "Fout bij het ophalen van traject." });
            }

            return Ok(userDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }


    //[HttpDelete("{id}")]
    //public void Delete(int id)
    //{

    //}

    private CreateUserDto UserDto(ApplicationUser user)
    {
        return new CreateUserDto
        {
            Id = user.Id,
            Username = user.UserName,
            Email = user.UserName,
            PatienId = user.PatienId,
            //Patient = user.Patient,
            TrajectId = user.Patient.TrajectId,
            PatientPathLocation = user.Patient.PathLocation
        };
    }
}

