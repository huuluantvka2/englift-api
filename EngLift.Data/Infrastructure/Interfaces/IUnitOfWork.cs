using EngLift.Data.Repository;

namespace EngLift.Data.Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        void SetUser(Guid userId);
        int SaveChanges();

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task SaveChangesAsync();

        #region Repo
        IWordRepository WordsRepo { get; }
        ILessonRepository LessonsRepo { get; }
        IUserRepository UsersRepo { get; }
        ICourseRepository CoursesRepo { get; }
        IUserRoleRepository UserRolesRepo { get; }
        ILessonWordRepository LessonWordRepo { get; }
        IUserLessonRepository UserLessonRepo { get; }
        #endregion
    }
}
