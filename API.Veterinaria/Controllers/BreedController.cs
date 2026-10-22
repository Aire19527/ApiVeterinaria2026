using API.Veterinaria.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Common.Resources;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO;
using Veterinaria.Entity.DTO.Breeds;

namespace API.Veterinaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class BreedController : ControllerBase
    {
        private readonly IBreedServices _breedServices;

        public BreedController(IBreedServices breedServices)
        {
            _breedServices = breedServices;
        }

        [HttpGet("GetAllBySpecieId")]
        public async Task<IActionResult> GetAllBySpecieId(int specieId)
        {
            List<BreedDto> result = await _breedServices.GetAllBySpecieId(specieId);
            return Ok(result);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] BreedAddDto breed)
        {
            IActionResult action;
            bool result = await _breedServices.Add(breed);
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
        public async Task<IActionResult> Update([FromBody] BreedUpdateDto breed)
        {
            bool result = await _breedServices.Update(breed);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? GeneralMessages.ItemUpdated : GeneralMessages.ItemNoUpdated
            };

            return result ? Ok(response) : BadRequest(response);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int breedId)
        {
            bool result = await _breedServices.Delete(breedId);
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
