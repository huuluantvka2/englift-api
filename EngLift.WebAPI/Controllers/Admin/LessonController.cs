using EngLift.Common;
using EngLift.DTO.Base;
using EngLift.DTO.Lesson;
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

        [HttpGet("Course/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_LESSON)]
        public async Task<IActionResult> GetAllLessonByCourseId(Guid Id, [FromQuery] BaseRequest request)
        {
            try
            {
                var result = await _lessonService.GetAllLessonByCourseId(Id, request);
                return Success<DataList<LessonItemDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"LessonController -> GetAllLessonByCourseId Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpGet("{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_LESSON)]
        public async Task<IActionResult> GetLessonDetail(Guid Id)
        {
            try
            {
                var result = await _lessonService.GetLessonDetail(Id);
                return Success<LessonItemDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"LessonController -> GetLessonDetail Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPost("")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CREATE_VIEW_LESSON)]
        public async Task<IActionResult> CreateLesson([FromBody] LessonCreateDTO dto)
        {
            try
            {
                var result = await _lessonService.CreateLesson(dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"LessonController -> CreateLesson Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPut("{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_EDIT_VIEW_LESSON)]
        public async Task<IActionResult> UpdateLesson(Guid Id, [FromBody] LessonUpdateDTO dto)
        {
            try
            {
                var result = await _lessonService.UpdateLesson(Id, dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"LessonController -> UpdateLesson Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpDelete("{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_DELETE_VIEW_LESSON)]
        public async Task<IActionResult> DeleteLesson(Guid Id)
        {
            try
            {
                var result = await _lessonService.DeleteLesson(Id);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"LessonController -> DeleteLesson Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
