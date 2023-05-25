using EngLift.DTO.User;
using EngLift.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace EngLift.WebAPI.Controllers.Admin
{
    [ApiController]
    [Route("Api/v1/[controller]s/Admin")]
    public class AuthController : ControllerApiBase
    {
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginDTO dto)
        {
            return NotFoundData<string>();
        }
    }
}
