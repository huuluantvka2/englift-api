namespace EngLift.Data.Infrastructure.Interfaces
{
    public interface IDbFactory : IDisposable
    {
        BuildDbContext Init();
    }
}
