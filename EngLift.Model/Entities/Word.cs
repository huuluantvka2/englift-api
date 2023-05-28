using EngLift.Model.Abstracts;
using EngLift.Model.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities
{
    [Table("Words")]
    public class Word : AuditBase, IEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [MaxLength(256)]
        public string? Audio { get; set; }
        [Required]
        [MaxLength(50)]
        public string Content { get; set; }
        [Required]
        [MaxLength(100)]
        public string Trans { get; set; }
        [Required]
        [MaxLength(256)]
        public string Example { get; set; }
        [Required]
        [MaxLength(50)]
        public string Phonetic { get; set; }
        [MaxLength(256)]
        public string? Image { get; set; }
        [MaxLength(50)]
        public string? Position { get; set; }
        public bool Active { get; set; } = true;

        public ICollection<LessonWord> LessonWords { get; set; }
    }
}
