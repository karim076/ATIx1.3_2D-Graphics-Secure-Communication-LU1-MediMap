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
    [Authorize(Roles = "User,Admin")]
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
    [Authorize(Roles = "User,Admin")]
    public async Task<ActionResult<CreateUserDto>> Put(int id, CreateUserDto updateUser)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var user = await _unitOfWork.ApplicationUserRepository.GetAsync(p => p.Id == id, includeProperty: "Patient");
            if (user == null)
            {
                return NotFound(new { message = "Onbekende user." });
            }

            var patient = await _unitOfWork.PatientRepository.GetAsync(p => p.Id == user.PatienId);

            if(patient == null)
            {
                return NotFound(new { message = "Geen patient om te updaten" });
            }

            if (user.Patient != null)
            {
                //patient.TrajectId = updateUser.TrajectId ?? 0;
                if(updateUser.PatientPathLocation > patient.PathLocation)
                {
                    patient.PathLocation = updateUser.PatientPathLocation;
                }
            }
            
            _unitOfWork.ApplicationUserRepository.Update(user);
            _unitOfWork.PatientRepository.Update(patient);

            await _unitOfWork.SaveAsync();

            return Ok(new {message = "Update succesvol"});
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }

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

