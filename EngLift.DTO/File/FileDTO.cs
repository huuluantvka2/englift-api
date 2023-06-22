namespace EngLift.DTO.File
{
    public enum FolderFile
    {
        COURSES = 1,
        LESSONS = 2,
        WORDS = 3
    }
    public class FileResponseDTO
    {
        public string Url { get; set; }
    }

    public class FileDeleteDTO
    {
        public string Path { get; set; }
    }
}
