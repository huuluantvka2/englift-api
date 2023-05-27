using EngLift.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities
{
    [Table("LessonWords")]
    public class LessonWord : IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public Guid LessonId { get; set; }
        [Required]
        public Guid WordId { get; set; }
        [ForeignKey("LessonId")]
        public virtual Lesson Lesson { get; set; }
        [ForeignKey("WordId")]
        public virtual Word Word { get; set; }

    }
}
