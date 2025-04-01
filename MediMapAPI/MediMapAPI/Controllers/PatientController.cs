using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Dto;
using System.ComponentModel.DataAnnotations;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MediMapAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PatientController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public PatientController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET api/<PatientController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDto>> Get(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {               
                var patient = await _unitOfWork.PatientRepository.GetAsync(p => p.Id == id, includeProperty:"Arts,OuderVoogd,Traject");

                if (patient == null)
                {
                    return NotFound(new { message = "Geen patient gevonden." });
                }
                var patientDto = ConvertToPatientDto(patient);
                if (patientDto == null)
                {
                    return BadRequest(new { message = "Fout bij het ophalen van patient." });
                }
                return Ok(patientDto);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        // POST api/<PatientController>
        [HttpPost]
        public async Task<ActionResult<PatientDto>> Post(PatientDto patientDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {

                var patient = GetPatient(patientDto);

                if (patient == null)
                {
                    return BadRequest(new { message = "Fout bij het ophalen van patient." });
                }
                await _unitOfWork.PatientRepository.AddAsync(patient);
                await _unitOfWork.SaveAsync();
                return Ok(patientDto);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDto>> Put(int id, PatientDto patientDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetAsync(p => p.Id == id, includeProperty:"Arts,Traject,OuderVoogd");
                if (patient == null)
                {
                    return NotFound(new { message = "Geen patient gevonden." });
                }
 
                patient.VoorNaam = patientDTO.VoorNaam;
                patient.AchterNaam = patientDTO.AchterNaam;
                patient.GeboorteDatum = patientDTO.GeboorteDatum;

                var patientDto = ConvertToPatientDto(patient);

                if (patientDto == null)
                {
                    return BadRequest(new { message = "Fout bij het ophalen van patient." });
                }

                _unitOfWork.PatientRepository.Update(patient);
                await _unitOfWork.SaveAsync();

                return Ok(patientDto);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPut("avatar/{Id}")]
        public async Task<ActionResult<AvatarName>> UpdateAvatar(int Id, AvatarName avatarName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetAsync(p => p.Id == Id);
                if (patient == null)
                {
                    return NotFound(new { message = "Geen patient gevonden." });
                }

                patient.AvatarNaam = avatarName.Avatar;

                _unitOfWork.PatientRepository.Update(patient);

                await _unitOfWork.SaveAsync();

                return Ok(new { avatar = patient.AvatarNaam});


            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpGet("avatar/{Id}")]
        public async Task<ActionResult<AvatarName>> GetAvatar(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var patient = await _unitOfWork.PatientRepository.GetAsync(p => p.Id == Id);
                if (patient == null)
                {
                    return NotFound(new { message = "Geen patient gevonden." });
                }
                return Ok(new { avatar = patient.AvatarNaam });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        //// DELETE api/<PatientController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}

        private PatientDto ConvertToPatientDto(Patient patient)
        {
            return new PatientDto
            {
                Id = patient.Id,
                VoorNaam = patient.VoorNaam,
                AchterNaam = patient.AchterNaam,
                AvatarNaam = patient.AvatarNaam,
                ArtsNaam = patient.Arts.Naam,
                TrajectNaam = patient.Traject.Naam,
                OuderVoogdNaam = $"{patient.OuderVoogd.VoorNaam} {patient.OuderVoogd.AchterNaam}",
                Afspraakatum = patient.AfspraakDatum,
                GeboorteDatum = patient.GeboorteDatum,
                //logbook = patient.LogBooks.Select(l => new LogBookDto
                //{
                //    Id = l.Id,
                //    Datum = l.Date,
                //    Omschrijving = l.Log,
                //    PatientId = l.PatientID
                //}).ToList() ?? new List<LogBookDto>()
            };
        }
        private Patient GetPatient(PatientDto patientDto)
        {
            return new Patient
            {
                VoorNaam = patientDto.VoorNaam,
                AchterNaam = patientDto.AchterNaam,
                GeboorteDatum = patientDto.GeboorteDatum,
                AvatarNaam = patientDto.AvatarNaam,
                OuderVoogdId = patientDto.OuderVoogdId,
                TrajectId = patientDto.TrajectId,
                ArtsId = patientDto.ArtsId
            };
        }

        public class AvatarName
        {
            [Required]
            public string Avatar { get; set; }
        }
    }
}
