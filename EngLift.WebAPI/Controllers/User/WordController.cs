using EngLift.DTO.Base;
using EngLift.DTO.Response;
using EngLift.DTO.Word;
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
    public class WordController : ControllerApiBase<WordController>
    {
        private readonly IWordService _wordService;
        public WordController(
            ILogger<WordController> logger,
            IWordService wordService
            ) : base(logger)
        {
            _wordService = wordService;
        }

        [HttpGet("{lessonId}")]
        public async Task<IActionResult> GetAllWordByLessonId(Guid lessonId, [FromQuery] BaseRequest request)
        {
            try
            {
                var result = await _wordService.GetAllWordUserByLessonId(lessonId, request);
                return Success<DataList<WordSearchResultDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> GetAllWordByLessonId Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [AllowAnonymous]
        [HttpGet("Search")]
        public async Task<IActionResult> SearchWord([FromQuery] SearchWordDTO request)
        {
            try
            {
                var result = await _wordService.SearchWordAsync(request);
                return Success<DataList<WordSearchResultDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> GetAllWordByLessonId Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
