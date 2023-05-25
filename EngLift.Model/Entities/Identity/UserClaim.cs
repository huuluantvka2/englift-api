using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    [Table("UserClaims")]
    public class UserClaim : IdentityUserClaim<Guid>
    {

    }
}
