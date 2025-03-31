using DataAccess.Repository.iUnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using Models.Model.Dto;

namespace MediMapAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OuderVoogdController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OuderVoogdController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<ActionResult<OuderVoogdDto>> Get(int id)
        {
            try
            {
                var ouderVoogd = await _unitOfWork.OuderVoogdRepository.GetAsync(o => o.Id == id);
                if (ouderVoogd == null)
                {
                    return NotFound(new { message = "OuderVoogd niet gevonden." });
                }

                var ouderVoogdDto = ConmvertDtoToModel(ouderVoogd);

                if (ouderVoogdDto == null)
                {
                    return BadRequest(new { message = "Fout bij het ophalen van ouderVoogd." });
                }

                return Ok(ouderVoogdDto);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<OuderVoogdDto>> Post(OuderVoogdDto ouderVoogdDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (ouderVoogdDto == null)
                {
                    return BadRequest(new { message = "OuderVoogd is leeg." });
                }

                var ouderVoogd = ConvertDtoToModel(ouderVoogdDto);

                await _unitOfWork.OuderVoogdRepository.AddAsync(ouderVoogd);
                await _unitOfWork.SaveAsync();
                return Ok(ouderVoogdDto);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        private OuderVoogdDto ConmvertDtoToModel(OuderVoogd dto)
        {
            return new OuderVoogdDto
            {
                Id = dto.Id,
                VoorNaam = dto.VoorNaam,
                AchterNaam = dto.AchterNaam,
            };
        }
        private OuderVoogd ConvertDtoToModel(OuderVoogdDto ouderVoogdDto)
        {
            return new OuderVoogd
            {
                VoorNaam = ouderVoogdDto.VoorNaam,
                AchterNaam = ouderVoogdDto.AchterNaam,
            };
        }
    }
}
