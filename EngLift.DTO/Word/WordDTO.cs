using EngLift.DTO.Base;
using EngLift.Model.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace EngLift.DTO.Word
{
    public class WordCreateDTO
    {
        public string Content { get; set; }
        public string Trans { get; set; }
        public string Example { get; set; }
        public string Phonetic { get; set; }
        public string? Image { get; set; }
        public string? Position { get; set; }
        public bool Active { get; set; } = true;
        public Guid LessonId { get; set; }
    }

    public class WordUpdateDTO
    {
        public string Content { get; set; }
        public string Trans { get; set; }
        public string Example { get; set; }
        public string Phonetic { get; set; }
        public string? Image { get; set; }
        public string? Position { get; set; }
        public bool Active { get; set; } = true;
    }
    public class WordItemDTO : AuditBase
    {
        public Guid Id { get; set; }
        public string? Audio { get; set; }
        public string Content { get; set; }
        public string Trans { get; set; }
        public string Example { get; set; }
        public string Phonetic { get; set; }
        public string? Image { get; set; }
        public string? Position { get; set; }
        public bool Active { get; set; } = true;
    }

    public class WordCreateExcelDTO : WordCreateDTO
    {
        public string MessageError { get; set; } = "";
        public string LessonId { get; set; }
    }
    public class WordExcelFile
    {
        public string Path { get; set; }
    }

    public class SearchWordDTO
    {
        [Required]
        public string Content { get; set; }
    }
    public class WordRequest : BaseRequest
    {
        public Guid? LessonId { get; set; }
    }

    public class WordSearchResultDTO
    {
        public Guid Id { get; set; }
        public string? Audio { get; set; }
        public string Content { get; set; }
        public string Trans { get; set; }
        public string Example { get; set; }
        public string Phonetic { get; set; }
        public string? Image { get; set; }
        public string? Position { get; set; }
    }

}
