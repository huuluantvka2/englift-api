using EngLift.Data.Infrastructure.Abstracts;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Model.Entities;

namespace EngLift.Data.Repository
{
    public interface ILessonWordRepository : IRepository<LessonWord, Guid> { }
    public class LessonWordRepository : RepositoryBase<LessonWord, Guid>, ILessonWordRepository
    {
        public LessonWordRepository(IDbFactory factory) : base(factory)
        {

        }
    }
}
