using EngLift.DTO.Base;
using EngLift.DTO.Course;
using EngLift.DTO.Response;
using EngLift.Model.Entities;
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

        [AllowAnonymous]
        [HttpGet("CourseMigration")]
        public async Task<IActionResult> GetCourseMigration()
        {
            try
            {
                var result = await _courseService.GetCourseMigration();
                return Success<List<Course>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> GetCourseMigration Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("LessonMigration")]
        public async Task<IActionResult> GetLessonMigration()
        {
            try
            {
                var result = await _courseService.GetLessonMigration();
                return Success<List<Lesson>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> GetLessonMigration Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("WordMigration")]
        public async Task<IActionResult> GetWordMigration()
        {
            try
            {
                var result = await _courseService.GetWordMigration();
                return Success<List<Word>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"CourseController -> GetWordMigration Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
