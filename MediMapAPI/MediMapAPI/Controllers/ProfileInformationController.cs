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
    public async Task<ActionResult<ProfileInformationDto>> GetProfileInformationByIdAsync(int id)
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
            var profileInformationDto = ProfileInformationDto(profileInformation);

            if (profileInformationDto == null)
            {
                return BadRequest(new { message = "Fout bij het ophalen van profiel informatie." });
            }

            return Ok(profileInformationDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    [HttpPost]
    public async Task<ActionResult<ProfileInformationDto>> CreateProfileInformationAsync(ProfileInformationDto profileInformationDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var profileInformation = ConvertToProfileInformation(profileInformationDto);
            if (profileInformation == null) 
            {
                return NotFound(new { message = "Geen profile informatie gevonden." });
            }

            await _unitOfWork.ProfileInformationRepository.AddAsync(profileInformation);
            await _unitOfWork.SaveAsync();
            return Ok(profileInformationDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    // PUT api/<ProfileInformationController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ProfileInformationDto>> UpdateProfileInformationAsync(int id, ProfileInformationDto profileInformationDto)
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

            _unitOfWork.ProfileInformationRepository.Update(profileInformation);
            await _unitOfWork.SaveAsync();
            return Ok(profileInformationDto);
        }
        catch (Exception e)
        {
            return BadRequest(new { message = e.Message });
        }
    }
    private ProfileInformation ConvertToProfileInformation(ProfileInformationDto profileInformationDto)
    {
        return new ProfileInformation
        {
            Id = profileInformationDto.Id,
            Naam = profileInformationDto.Naam,
            GeboorteDatum = profileInformationDto.GeboorteDatum,
            NaamDokter = profileInformationDto.NaamDokter,
            BehandelPlan = profileInformationDto.BehandelPlan,
            AfspraakDatum = profileInformationDto.AfspraakDatum,
            PatientId = profileInformationDto.PatientId,
            ArtsId = profileInformationDto.ArtsId
        };
    }
    private ProfileInformationDto ProfileInformationDto(ProfileInformation profileInformation)
    {
        return new ProfileInformationDto
        {
            Id = profileInformation.Id,
            Naam = profileInformation.Naam,
            GeboorteDatum = profileInformation.GeboorteDatum,
            NaamDokter = profileInformation.NaamDokter,
            BehandelPlan = profileInformation.BehandelPlan,
            AfspraakDatum = profileInformation.AfspraakDatum,
            PatientId = profileInformation.PatientId,
            ArtsId = profileInformation.ArtsId
        };
    }
}
