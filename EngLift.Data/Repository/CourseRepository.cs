using EngLift.Data.Infrastructure.Abstracts;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Model.Entities;

namespace EngLift.Data.Repository
{
    public interface ICourseRepository : IRepository<Course, Guid>
    {

    }
    public class CourseRepository : RepositoryBase<Course, Guid>, ICourseRepository
    {
        public CourseRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}
