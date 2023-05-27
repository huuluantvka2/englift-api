using EngLift.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    public enum GROUP_ROLE
    {
        Admin = 1,
        Course = 2,
        Word = 3,
        Lesson = 4,
        User = 5,
    }
    [Table("Roles")]
    public class Role : IdentityRole<Guid>, IEntity<Guid>
    {
        public GROUP_ROLE Group { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
