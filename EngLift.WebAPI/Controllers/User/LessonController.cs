﻿using EngLift.DTO.Base;
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
        [HttpGet("Courses/{courseId}")]
        public async Task<IActionResult> GetAllLessonUserByCourseId(Guid courseId, [FromQuery] BaseRequest request)
        {
            try
            {
                var result = await _lessonService.GetAllLessonUserByCourseId(courseId, request);
                return Success<DataList<LessonItemUserDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"LessonController -> GetAllCorseUser Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpGet("{lessonId}")]
        public async Task<IActionResult> GetLessonUserById(Guid lessonId)
        {
            try
            {
                var result = await _lessonService.GetLessonUserById(lessonId);
                return Success<LessonItemUserDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"LessonController -> GetLessonUserById Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
