using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Dto;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MediMapAPI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
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
        public void Post([FromBody]string value)
        {

        }

        // PUT api/<PatientController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<PatientController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

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
                //logbook = patient.LogBooks.Select(l => new LogBookDto
                //{
                //    Id = l.Id,
                //    Datum = l.Date,
                //    Omschrijving = l.Log,
                //    PatientId = l.PatientID
                //}).ToList() ?? new List<LogBookDto>()
            };
        }
    }
}
