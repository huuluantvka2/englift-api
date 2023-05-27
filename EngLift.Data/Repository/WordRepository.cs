using EngLift.Data.Infrastructure.Abstracts;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Model.Entities;

namespace EngLift.Data.Repository
{
    public interface IWordRepository : IRepository<Word, Guid> { }
    public class WordRepository : RepositoryBase<Word, Guid>, IWordRepository
    {
        public WordRepository(IDbFactory factory) : base(factory)
        {

        }
    }
}
