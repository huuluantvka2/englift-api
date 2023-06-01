using EngLift.Common;
using EngLift.DTO.Base;
using EngLift.DTO.Response;
using EngLift.DTO.Word;
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

        [HttpGet("Lesson/{Id}/Words")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_WORD)]
        public async Task<IActionResult> GetAllWordByLessonId(Guid Id, [FromQuery] BaseRequest request)
        {
            try
            {
                var result = await _wordService.GetAllWordByLessonId(Id, request);
                return Success<DataList<WordItemDTO>>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> GetAllWordByLessonId Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpGet("Words/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CAN_VIEW_WORD)]
        public async Task<IActionResult> GetWordDetail(Guid Id)
        {
            try
            {
                var result = await _wordService.GetWordDetail(Id);
                return Success<WordItemDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> GetWordDetail Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPost("Words")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_CREATE_VIEW_WORD)]
        public async Task<IActionResult> CreateWord([FromBody] WordCreateDTO dto)
        {
            try
            {
                var result = await _wordService.CreateWord(dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> CreateWord Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpPut("Words/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_EDIT_VIEW_WORD)]
        public async Task<IActionResult> UpdateWord(Guid Id, [FromBody] WordUpdateDTO dto)
        {
            try
            {
                var result = await _wordService.UpdateWord(Id, dto);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> UpdateWord Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        [HttpDelete("Words/{Id}")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN, RolesName.ROLE_USER_DELETE_VIEW_WORD)]
        public async Task<IActionResult> DeleteWord(Guid Id)
        {
            try
            {
                var result = await _wordService.DeleteWord(Id);
                return Success<SingleId>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"WordController -> DeleteWord Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
