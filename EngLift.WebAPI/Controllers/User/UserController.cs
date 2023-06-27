using EngLift.DTO.User;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using EngLift.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngLift.WebAPI.Controllers.User
{
    [Authorize]
    [ApiController]
    [Route("Api/v1/[controller]s")]
    public class UserController : ControllerApiBase<UserController>
    {
        private readonly IUserService _userService;
        public UserController(
            ILogger<UserController> logger,
            IUserService userService
            ) : base(logger)
        {
            _userService = userService;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                string UserId = User.Claims.Where(x => x.Type == "UserId").First().Value;
                var result = await _userService.GetUserById(Guid.Parse(UserId));
                return Success<UserItemDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"AuthController -> GetProfile Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
