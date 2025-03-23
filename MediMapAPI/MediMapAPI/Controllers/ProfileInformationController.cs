using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Dto;

namespace MediMapAPI.Controllers;

//[Authorize]
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
    [HttpGet("{id}")]
    public async Task<ActionResult<ProfileInformationDto>> GetByIdAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var profileInformation = await _unitOfWork.ProfileInformationRepository.GetAsync(p => p.Id == id);
            if (profileInformation == null)
            {
                return NotFound(new { message = "Geen profile informatie gevonden." });
            }
            ProfileInformationDto profileInformationDto = new()
            {
                Id = profileInformation.Id,
                Naam = profileInformation.Naam,
                GeboorteDatum = profileInformation.GeboorteDatum,
                NaamDokter = profileInformation.NaamDokter,
                BehandelPlan = profileInformation.BehandelPlan,
                AfspraakDatum = profileInformation.AfspraakDatum,
                PatientId = profileInformation.PatientId
            };
            return Ok(profileInformationDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    // PUT api/<ProfileInformationController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ProfileInformationDto>> Put(int id, ProfileInformationDto profileInformationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var profileInformation = await _unitOfWork.ProfileInformationRepository.GetAsync(p => p.Id == id);
            if (profileInformation == null)
            {
                return NotFound(new { message = "Geen profile informatie gevonden." });
            }

            profileInformation.Naam = profileInformationDto.Naam;
            profileInformation.GeboorteDatum = profileInformationDto.GeboorteDatum;
            profileInformation.NaamDokter = profileInformationDto.NaamDokter;
            profileInformation.BehandelPlan = profileInformationDto.BehandelPlan;
            profileInformation.AfspraakDatum = profileInformationDto.AfspraakDatum;
            profileInformation.PatientId = profileInformationDto.PatientId;
            _unitOfWork.ProfileInformationRepository.Update(profileInformation);
            await _unitOfWork.SaveAsync();
            return Ok(profileInformationDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
        

    }
}
