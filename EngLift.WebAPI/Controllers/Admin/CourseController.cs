using EngLift.Common;
using EngLift.DTO.Base;
using EngLift.DTO.Course;
using EngLift.DTO.Response;
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
    public class CourseController : ControllerApiBase<CourseController>
    {
        private readonly ICourseService _courseService;
        public CourseController(
            ILogger<CourseController> logger,
            ICourseService courseService
            ) : base(logger)
        {
            _courseService = courseService;
        }

        [HttpGet("Courses")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_COURSE)]
        public async Task<IActionResult> GetAllCourse([FromQuery] BaseRequest request)
        {
            try
            {
                var result = await _courseService.GetAllCourse(request);
                return Success<DataList<CourseItemDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> GetAllCourse Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpGet("Courses/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_COURSE)]
        public async Task<IActionResult> GetCourseDetail(Guid Id)
        {
            try
            {
                var result = await _courseService.GetCourseDetail(Id);
                return Success<CourseItemDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> GetCourseDetail Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPost("Courses")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CREATE_VIEW_COURSE)]
        public async Task<IActionResult> CreateCourse([FromBody] CourseCreateDTO dto)
        {
            try
            {
                var result = await _courseService.CreateCourse(dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> CreateCourse Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPut("Courses/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_EDIT_VIEW_COURSE)]
        public async Task<IActionResult> UpdateCourse(Guid Id, [FromBody] CourseUpdateDTO dto)
        {
            try
            {
                var result = await _courseService.UpdateCourse(Id, dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> UpdateCourse Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpDelete("Courses/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_DELETE_VIEW_COURSE)]
        public async Task<IActionResult> DeleteCourse(Guid Id)
        {
            try
            {
                var result = await _courseService.DeleteCourse(Id);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> DeleteCourse Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
