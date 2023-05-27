using EngLift.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    [Table("UserRoles")]
    public class UserRole : IdentityUserRole<Guid>, IEntity<Guid>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
