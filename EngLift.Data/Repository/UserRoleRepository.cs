using EngLift.Data.Infrastructure.Abstracts;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Model.Entities.Identity;

namespace EngLift.Data.Repository
{
    public interface IUserRoleRepository : IRepository<UserRole, Guid> { }
    public class UserRoleRepository : RepositoryBase<UserRole, Guid>, IUserRoleRepository
    {
        public UserRoleRepository(IDbFactory factory) : base(factory)
        {

        }
    }
}
