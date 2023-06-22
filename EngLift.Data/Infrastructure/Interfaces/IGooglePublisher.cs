namespace EngLift.Data.Infrastructure.Interfaces
{
    public interface IGooglePublisher
    {
        public Task SendMessageAsync<T>(T data);

    }
}
