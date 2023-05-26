using EngLift.Model.Abstracts;
using EngLift.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities
{
    [Table("Lessons")]
    public class Lesson : AuditBase, IEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string? Author { get; set; }

        [MaxLength(256)]
        public string? Desciption { get; set; }
        public int? Prior { get; set; } = 0; //số càng cao càng xếp lên trước
        public int? Viewed { get; set; } = 0;
        [MaxLength(256)]
        public string? Image { get; set; }
        public bool Active { get; set; } = true;

    }
}
