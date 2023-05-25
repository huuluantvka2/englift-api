using EngLift.Data.Repository;

namespace EngLift.Data.Infrastructure.Interfaces
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbFactory _dbFactory { set; get; }
        protected BuildDbContext DbContext { get { return _dbFactory.Init(); } }
        private Guid userId { get; set; }

        public UnitOfWork(IDbFactory dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public void BeginTransaction()
        {
            DbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            DbContext.Database.CommitTransaction();
        }

        public void RollbackTransaction()
        {
            DbContext.Database.RollbackTransaction();
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public Task SaveChangesAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        public void SetUser(Guid userId)
        {
            this.userId = userId;
        }

        #region InitAndGetRepo
        private IWordRepository wordsRepo { get; set; }
        public IWordRepository WordsRepo => wordsRepo ?? (new WordRepository(_dbFactory));

        private ILessonRepository lessonsRepo { get; set; }
        public ILessonRepository LessonsRepo => lessonsRepo ?? (new LessonRepository(_dbFactory));

        #endregion
    }
}
