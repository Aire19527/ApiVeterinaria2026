using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Common.Resources;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO;
using Veterinaria.Entity.DTO.Species;

namespace API.Veterinaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SpeciesController : ControllerBase
    {
        private readonly ISpeciesServices _speciesServices;

        public SpeciesController(ISpeciesServices speciesServices)
        {
            _speciesServices = speciesServices;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<SpecieDto> result = await _speciesServices.GetAll();
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] string name)
        {
            IActionResult action;
            bool result = await _speciesServices.Add(name);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? GeneralMessages.ItemInserted : GeneralMessages.ItemNoInserted
            };

            if (result)
                action = Ok(response);
            else
                action = BadRequest(response);

            return action;
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] SpecieDto specie)
        {
            bool result = await _speciesServices.Update(specie);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? GeneralMessages.ItemUpdated : GeneralMessages.ItemNoUpdated
            };

            return result ? Ok(response) : BadRequest(response);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int specieId)
        {
            bool result = await _speciesServices.Delete(specieId);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted
            };

            return result ? Ok(response) : BadRequest(response);
        }

    }
}
