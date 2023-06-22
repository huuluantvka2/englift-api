namespace EngLift.Data.Infrastructure.Interfaces
{
    public interface IGoogleSucscriberFactory
    {
        public IGoogleSubscriber Init(string topicName);
    }
}
