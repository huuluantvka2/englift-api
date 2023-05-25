using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    [Table("UserRoles")]
    public class UserRole : IdentityUserRole<Guid>
    {

    }
}
