using EngLift.Model.Entities.Identity;
using EngLift.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities
{
    public enum LevelLesson
    {
        LevelOne = 1,
        LevelTwo = 2,
        LevelThree = 3,
        LevelFour = 4,
        LevelFive = 5,
    }

    [Table("UserLessons")]
    public class UserLesson : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public Guid LessonId { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; }
        public LevelLesson Level { get; set; } = LevelLesson.LevelOne;
        public int TotalWords { get; set; } = 0;
        public DateTime? NextTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
