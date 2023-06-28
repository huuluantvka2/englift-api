using EngLift.DTO.Base;
using EngLift.DTO.Lesson;
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
    public class LessonController : ControllerApiBase<LessonController>
    {
        private readonly ILessonService _lessonService;
        public LessonController(
            ILogger<LessonController> logger,
            ILessonService lessonService
            ) : base(logger)
        {
            _lessonService = lessonService;
        }

        [AllowAnonymous]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetAllCorseUser(Guid courseId, [FromQuery] BaseRequest request)
        {
            try
            {
                var result = await _lessonService.GetAllLessonUserByCourseId(courseId, request);
                return Success<DataList<LessonItemUserDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"GetAllCorseUser -> GetAllCorseUser Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
