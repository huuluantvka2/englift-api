using EngLift.Model.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace EngLift.Model.Entities.Identity
{
    [Table("Roles")]
    public class Role : IdentityRole<Guid>, IEntity<Guid>
    {

    }
}
