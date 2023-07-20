using EngLift.Model.Abstracts;

namespace EngLift.DTO.Course
{
    public class CourseCreateDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Prior { get; set; } = 0;
        public string? Image { get; set; }
        public bool Active { get; set; }
    }

    public class CourseUpdateDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Prior { get; set; } = 0;
        public string? Image { get; set; }
        public bool Active { get; set; }
    }
    public class CourseItemDTO : AuditBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? Prior { get; set; } = 0;
        public string? Image { get; set; }
        public int TotalLesson { get; set; }
        public bool Active { get; set; }
    }

    public class CourseItemPublicDTO : AuditBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
    }

}
