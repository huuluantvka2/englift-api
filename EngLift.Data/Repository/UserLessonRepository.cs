using EngLift.Data.Infrastructure.Abstracts;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Model.Entities;

namespace EngLift.Data.Repository
{
    public interface IUserLessonRepository : IRepository<UserLesson, Guid>
    {

    }
    public class UserLessonRepository : RepositoryBase<UserLesson, Guid>, IUserLessonRepository
    {
        public UserLessonRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
