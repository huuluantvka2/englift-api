using EngLift.Model.Abstracts;

namespace EngLift.DTO.Lesson
{
    public class LessonCreateDTO
    {
        public string Name { get; set; }
        public string? Author { get; set; }
        public string? Desciption { get; set; }
        public int? Prior { get; set; } = 0;
        public string? Image { get; set; }
        public bool Active { get; set; } = true;
        public Guid CourseId { set; get; }
    }

    public class LessonUpdateDTO
    {
        public string Name { get; set; }
        public string? Author { get; set; }
        public string? Desciption { get; set; }
        public int? Prior { get; set; } = 0;
        public string? Image { get; set; }
        public bool Active { get; set; } = true;
    }
    public class LessonItemDTO : AuditBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Author { get; set; }
        public string? Desciption { get; set; }
        public int? Prior { get; set; } = 0;
        public int? Viewed { get; set; } = 0;
        public string? Image { get; set; }
        public bool Active { get; set; } = true;
        public Guid CourseId { set; get; }
    }
}
