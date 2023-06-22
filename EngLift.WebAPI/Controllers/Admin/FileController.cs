using EngLift.Common;
using EngLift.DTO.File;
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
    public class FileController : ControllerApiBase<FileController>
    {
        private readonly IFileService _fileService;
        public FileController(
            ILogger<FileController> logger,
            IFileService fileService
            ) : base(logger)
        {
            _fileService = fileService;
        }

        /// <summary>
        /// Single File Upload
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN)]
        public IActionResult PostFileAsync([FromForm] IFormFile file, FolderFile type)
        {
            try
            {
                if (file == null || type == null)
                {
                    return BadRequest("Invalid file");
                }
                var result = _fileService.PostFileAsync(file, type);
                return Success<FileResponseDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"FileController -> PostFileAsync Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }

        /// <summary>
        /// Delete file
        /// </summary>
        [HttpDelete("")]
        [RolesAllow(RolesName.ROLE_SYSTEM_ADMIN)]
        public IActionResult RemoveFileAsync([FromQuery] string path)
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    return BadRequest("Invalid file");
                }
                var result = _fileService.RemoveFileAsync(path);
                return Success<FileResponseDTO>(result);
            }
            catch (ServiceExeption ex)
            {
                _logger.LogInformation($"FileController -> PostFileAsync Throw Exception: {ex.Message}");
                return HandleError(ex);
            }
        }
    }
}
