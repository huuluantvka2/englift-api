using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    [Table("UserTokens")]
    public class UserToken : IdentityUserToken<Guid>
    {

    }
}
