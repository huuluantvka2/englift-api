using EngLift.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    [Table("Users")]
    public class User : IdentityUser<Guid>, IEntity<Guid>, IAudit
    {
        public DateTime? CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public bool? IsAdmin { get; set; } = false;
    }
}
