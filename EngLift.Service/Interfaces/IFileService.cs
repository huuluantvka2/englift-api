using EngLift.DTO.File;
using Microsoft.AspNetCore.Http;

namespace EngLift.Service.Interfaces
{
    public interface IFileService
    {
        public FileResponseDTO PostFileAsync(IFormFile file, FolderFile type);
        public FileResponseDTO RemoveFileAsync(string path);
    }
}
