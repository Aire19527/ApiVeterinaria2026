using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Veterinaria.Domain.Services.Interfaces;

namespace API.Veterinaria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolController : ControllerBase
    {
        private readonly IRolServices _rolServices;

        public RolController(IRolServices rolServices)
        {
            this._rolServices = rolServices;
        }


        [HttpGet(Name = "GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _rolServices.GetAll();
            return Ok(result);
        }

    }

}
