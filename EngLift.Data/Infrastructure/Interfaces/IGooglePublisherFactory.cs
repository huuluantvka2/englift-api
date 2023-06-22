namespace EngLift.Data.Infrastructure.Interfaces
{
    public interface IGooglePublisherFactory
    {
        public IGooglePublisher Init(string topicName);
    }
}
