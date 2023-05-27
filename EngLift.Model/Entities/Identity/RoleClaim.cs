using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    [Table("RoleClaims")]
    public class RoleClaim : IdentityRoleClaim<Guid>
    {

    }
}
