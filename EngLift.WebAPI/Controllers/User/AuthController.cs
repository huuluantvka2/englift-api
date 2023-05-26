using EngLift.DTO.User;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using EngLift.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace EngLift.WebAPI.Controllers.User
{
    [ApiController]
    [Route("Api/v1/[controller]s/")]
    public class AuthController : ControllerApiBase<AuthController>
    {
        private readonly IAuthService _authService;
        public AuthController(
            ILogger<AuthController> logger,
            IAuthService authService
            ) : base(logger)
        {
            _authService = authService;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            try
            {
                var result = await _authService.LoginAdmin(dto);
                return Success<LoginSuccessDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"AuthService -> LoginAdmin Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
