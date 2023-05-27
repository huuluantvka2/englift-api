using EngLift.Common;
using EngLift.DTO.Base;
using EngLift.DTO.Response;
using EngLift.DTO.User;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using EngLift.WebAPI.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EngLift.WebAPI.Controllers.Admin
{
    [Authorize]
    [ApiController]
    [Route("Api/v1/[controller]s/Admin")]
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

        [HttpGet("Users")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_USER)]
        public async Task<IActionResult> GetAllUser([FromQuery] BaseRequest request)
        {
            try
            {
                var result = await _userService.GetAllUser(request);
                return Success<DataList<UserItemDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"UserController -> GetAllUser Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPut("Users/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_EDIT_VIEW_USER)]
        public async Task<IActionResult> AdminUpdateUser(Guid Id, [FromBody] UserAdminUpdateDTO dto)
        {
            try
            {
                var result = await _userService.AdminUpdateUser(Id, dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"UserController -> AdminUpdateUser Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpDelete("Users/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_EDIT_VIEW_USER)]
        public async Task<IActionResult> AdminDeleteUser(Guid Id)
        {
            try
            {
                var result = await _userService.AdminDeleteUser(Id);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"UserController -> AdminDeleteUser Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
