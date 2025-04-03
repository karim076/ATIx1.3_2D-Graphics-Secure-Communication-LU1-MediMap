using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.Model;
using Models.Model.Dto;



namespace MediMapAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogBookController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public LogBookController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> AddLog(LogBook log)
        {
            if (log == null) {
                return BadRequest("Log is null.");
            }
            
            await _unitOfWork.LogBookRepository.AddAsync(log);
            await _unitOfWork.SaveAsync();
            return Ok($"New log added: {log}");
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult<IEnumerable<LogBook>>> GetLogsbyPatientId(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var logs = await _unitOfWork.LogBookRepository.GetById(log => log.PatientID == id);

            if (logs == null)
            {
                return NotFound();
            }

            List<LogBookDTO> _logBookDTO = new List<LogBookDTO>();

            foreach (var log in logs)
            {
                _logBookDTO.Add(new LogBookDTO
                {
                    Place = log.Place,
                    Date = log.Date,
                    Note = log.Note,
                    Id = log.Id
                });
            }

            return Ok(_logBookDTO);
        }


        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteLog(LogBook id)
        {
            if (id == null)
            {
                return BadRequest("Could not find id.");
            }

            _unitOfWork.LogBookRepository.Delete(id);
            await _unitOfWork.SaveAsync();
            return Ok($"Log with ID {id} deleted.");
        }

    }
}
