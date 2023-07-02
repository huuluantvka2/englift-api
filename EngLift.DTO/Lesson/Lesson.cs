using EngLift.Model.Abstracts;
using EngLift.Model.Entities;

namespace EngLift.DTO.Lesson
{
    public class LessonCreateDTO
    {
        public string Name { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public int? Prior { get; set; } = 0;
        public string? Image { get; set; }
        public bool Active { get; set; } = true;
        public Guid CourseId { set; get; }
    }

    public class LessonUpdateDTO
    {
        public string Name { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public int? Prior { get; set; } = 0;
        public string? Image { get; set; }
        public bool Active { get; set; } = true;
    }
    public class LessonItemDTO : AuditBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public int? Prior { get; set; } = 0;
        public int TotalWords { get; set; } = 0;
        public int? Viewed { get; set; } = 0;
        public string? Image { get; set; }
        public bool Active { get; set; } = true;
        public Guid CourseId { set; get; }
    }

    public class LessonItemUserDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public int? Viewed { get; set; } = 0;
        public string? Image { get; set; }
        public Guid CourseId { set; get; }
        public LevelLesson? LevelLesson { get; set; }
        public DateTime? LastTimeStudy { get; set; }
        public DateTime? NextTime { get; set; }
    }

    public class UserLessonDTO
    {
        /*        public Guid LessonId { get; set; }*/
    }
}
