using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.File;
using EngLift.Service.Extensions;
using EngLift.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EngLift.Service.Implements
{
    public class FileService : ServiceBase<FileService>, IFileService
    {
        public FileService(ILogger<FileService> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
        }

        public FileResponseDTO PostFileAsync(IFormFile file, FolderFile type)
        {
            _logger.LogInformation($"FileService -> PostFileAsync with fileName {file.FileName}");
            var extension = Path.GetExtension(file.FileName);

            if (!(new string[] { ".jpg", ".png" }.Contains(extension)))
            {
                throw new ServiceExeption(HttpStatusCode.BadRequest, ErrorMessage.ALLOWPNGORJPG);
            };
            var uniqueNameFile = $"{Guid.NewGuid()}_{file.FileName}";
            var folder = Path.Combine("uploads", Enum.GetName(type).ToLower());
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folder);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            var fullPath = Path.Combine(directory, uniqueNameFile);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
            _logger.LogInformation($"FileService -> PostFileAsync with fileName {file.FileName} successfully");
            return new FileResponseDTO() { Url = Path.Combine(folder, uniqueNameFile) };
        }

        public FileResponseDTO RemoveFileAsync(string path)
        {
            try
            {
                _logger.LogInformation($"FileService -> RemoteFileAsync with path {path}");
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path);
                File.Delete(fullPath);
                _logger.LogInformation($"FileService -> RemoteFileAsync with path {path} successfully");
                return new FileResponseDTO() { Url = path };
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"FileService -> RemoteFileAsync with path {path} error");
                throw new ServiceExeption(HttpStatusCode.BadRequest, message: ex.Message);
            }
        }
    }
}
