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
        public IWordRepository WordsRepo => wordsRepo ?? (wordsRepo = new WordRepository(_dbFactory));

        private ILessonRepository lessonsRepo { get; set; }
        public ILessonRepository LessonsRepo => lessonsRepo ?? (lessonsRepo = new LessonRepository(_dbFactory));

        private IUserRepository usersRepo { get; set; }
        public IUserRepository UsersRepo => usersRepo ?? (usersRepo = new UserRepository(_dbFactory));
        private ICourseRepository coursesRepo { get; set; }
        public ICourseRepository CoursesRepo => coursesRepo ?? (coursesRepo = new CourseRepository(_dbFactory));
        private IUserRoleRepository userRolesRepo { get; set; }
        public IUserRoleRepository UserRolesRepo => userRolesRepo ?? (userRolesRepo = new UserRoleRepository(_dbFactory));
        private ILessonWordRepository lessonWordRepo { get; set; }
        public ILessonWordRepository LessonWordRepo => lessonWordRepo ?? (lessonWordRepo = new LessonWordRepository(_dbFactory));

        #endregion
    }
}
