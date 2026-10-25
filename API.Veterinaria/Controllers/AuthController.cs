using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Domain.Services.Interfaces;
using Veterinaria.Entity.DTO;
using Veterinaria.Entity.DTO.Tokens;

namespace API.Veterinaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Attributes
        private readonly IAuthServices _authServices;
        #endregion

        #region Builder
        public AuthController(IAuthServices authServices)
        {
            this._authServices = authServices;
        }
        #endregion

        #region Services


        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto login)
        {
            TokenDto result = await _authServices.Login(login);
            ResponseDto response = new ResponseDto()
            {
                IsSuccess = true,
                Result = result,
                Message = string.Empty
            };

            return Ok(response);
        }



        #endregion
    }

}
