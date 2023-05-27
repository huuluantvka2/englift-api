using EngLift.Data.Infrastructure.Abstracts;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Model.Entities.Identity;

namespace EngLift.Data.Repository
{
    public interface IUserRepository : IRepository<User, Guid> { }
    public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
    {
        public UserRepository(IDbFactory factory) : base(factory)
        {

        }
    }
}
