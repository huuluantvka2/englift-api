using EngLift.Model.Abstracts;
using EngLift.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities
{

    [Table("Courses")]
    public class Course : AuditBase, IEntity<Guid>
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string? Description { get; set; }
        public int? Prior { get; set; } = 0; //số càng cao càng xếp lên trước
        [MaxLength(256)]
        public string? Image { get; set; }
        public bool Active { get; set; } = true;

        public virtual ICollection<Lesson> Lessons { get; set; }
    }
}
