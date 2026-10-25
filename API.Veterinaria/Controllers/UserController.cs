using API.Veterinaria.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Common.Resources;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO;
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

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserUpdateDto user)
        {
            bool result = await _userServices.Update(user);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? GeneralMessages.ItemUpdated : GeneralMessages.ItemNoUpdated
            };

            return result ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int userId)
        {
            bool result = await _userServices.Delete(userId);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = result,
                Result = result,
                Message = result ? GeneralMessages.ItemDeleted : GeneralMessages.ItemNoDeleted
            };

            return result ? Ok(response) : BadRequest(response);
        }

        #endregion
    }
}
