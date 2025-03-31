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
    public class ArtsController : ControllerBase
    {

        private readonly IUnitOfWork unitOfWork;

        public ArtsController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArtsDto>> Get(int Id)
        {
            try
            {
                var arts = await unitOfWork.ArtsRepository.GetAsync(a => a.Id == Id);
                if (arts == null)
                {
                    return NotFound(new { message = "Geen arts gevonden." });
                }
                var artsDto = GetArtsDto(arts);
                return Ok(artsDto);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ArtsDto>> PostArts(ArtsDto artsDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (artsDto == null)
                {
                    return BadRequest(new { message = "Arts is leeg." });
                }

                var arts = new Arts
                {
                    Naam = artsDto.Naam,
                    Specialisatie = artsDto.Specialisatie
                };
                await unitOfWork.ArtsRepository.AddAsync(arts);
                await unitOfWork.SaveAsync();

                if(arts.Id == 0)
                {
                    return BadRequest(new { message = "Arts is niet toegevoegd." });
                }
                var result = GetArtsDto(arts);

                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
        private ArtsDto GetArtsDto(Arts arts)
        {
            return new ArtsDto
            {
                Id = arts.Id,
                Naam = arts.Naam,
                Specialisatie = arts.Specialisatie
            };
        }
    }
}
