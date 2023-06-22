namespace EngLift.Data.Infrastructure.Interfaces
{
    public interface IGoogleSubscriber
    {
        public Task<List<T>> ProcessMessage<T>();

    }
}
