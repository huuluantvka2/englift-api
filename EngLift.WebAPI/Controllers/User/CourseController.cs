using EngLift.DTO.Base;
using EngLift.DTO.Course;
using EngLift.DTO.Response;
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

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> GetAllCorseUser([FromQuery] BaseRequest request)
        {
            try
            {
                var result = await _courseService.GetAllCorseUser(request);
                return Success<DataList<CourseItemPublicDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> GetAllCorseUser Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetCourseUserById(Guid Id)
        {
            try
            {
                var result = await _courseService.GetCourseUserById(Id);
                return Success<CourseItemPublicDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> GetCourseUserById Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        /*        [AllowAnonymous]
                [HttpGet("Migration")]
                public async Task<IActionResult> MigrationCourse()
                {
                    try
                    {
                        await _courseService.MigrationCourse();
                        return Ok();
                    }
                    catch (ServiceExeption ex)
                    {
                        _logger.LogInformation($"CourseController -> MigrationCourse Throw Exception: {ex.Message}");
                        return HandleError(ex);
                    }
                }*/
    }
}
