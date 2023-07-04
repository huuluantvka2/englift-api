using EngLift.DTO.Response;
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
                _logger.LogInformation($"UserController -> GetProfile Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPut("Profile")]
        public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto dto)
        {
            try
            {
                string UserId = User.Claims.Where(x => x.Type == "UserId").First().Value;
                var result = await _userService.UpdateUser(Guid.Parse(UserId), dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"UserController -> UpdateUser Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPost("ReportWords")]
        public async Task<IActionResult> GetReportWords([FromBody] ReportWordDto dto)
        {
            try
            {
                string UserId = User.Claims.Where(x => x.Type == "UserId").First().Value;
                var result = await _userService.GetReportWords(Guid.Parse(UserId), dto);
                return Success<ReportWordData>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"UserController -> GetReportWords Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
