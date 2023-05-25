using EngLift.Data.Infrastructure.Abstracts;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Model.Entities;

namespace EngLift.Data.Repository
{
    public interface ILessonRepository : IRepository<Lesson, Guid> { }
    public class LessonRepository : RepositoryBase<Lesson, Guid>, ILessonRepository
    {
        public LessonRepository(IDbFactory factory) : base(factory)
        {

        }
    }
}
