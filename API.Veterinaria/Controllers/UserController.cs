using API.Veterinaria.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Common.Resources;
using Veterinaria.Domain.Services;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO;
using Veterinaria.Entity.DTO.Species;
using Veterinaria.Entity.DTO.Users;

namespace API.Veterinaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [TypeFilter(typeof(CustomExceptionHandler))]
    public class UserController : ControllerBase
    {
        #region Attributes
        private readonly IUserServices _userServices;
        #endregion

        #region Builder
        public UserController(IUserServices userServices)
        {
            this._userServices = userServices;
        }
        #endregion

        #region Services

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            List<UserDto> result = await _userServices.GetAll();
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserAddDto userAdd)
        {
            bool result = await _userServices.Add(userAdd);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? GeneralMessages.ItemInserted : GeneralMessages.ItemNoInserted
            };

            return result ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
