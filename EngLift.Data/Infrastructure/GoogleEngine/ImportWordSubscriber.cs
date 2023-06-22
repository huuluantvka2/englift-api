using EngLift.Common;
using EngLift.Data.Infrastructure.Factories;
using EngLift.Data.Infrastructure.Interfaces;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EngLift.Data.Infrastructure.GoogleEngine
{
    internal class ImportWordSubscriber : IGoogleSubscriber
    {
        private SubscriptionName subscriptionName = new SubscriptionName(GoogleConstant.GoogleIdProject, GoogleConstant.PubSubSubscriptionImport);
        private ILogger<GoogleSubscriberFactory> _logger;
        public ImportWordSubscriber(ILogger<GoogleSubscriberFactory> logger)
        {
            _logger = logger;
        }

        public async Task<List<T>> ProcessMessage<T>()
        {
            try
            {
                _logger.LogInformation($"ImportWordSubscriber -> ProcessMessage");
                SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);
                List<T> list = new List<T>();
                await subscriber.StartAsync((msg, cancellationToken) =>
                {
                    _logger.LogInformation($"Received message {msg.MessageId} published at {msg.PublishTime.ToDateTime()}");
                    list = JsonConvert.DeserializeObject<List<T>>(msg.Data.ToStringUtf8());
                    subscriber.StopAsync(TimeSpan.FromSeconds(15));
                    return Task.FromResult(SubscriberClient.Reply.Ack);
                });
                _logger.LogInformation($"ImportWordSubscriber -> ProcessMessage successfully");
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"ImportWordSubscriber -> ProcessMessage Throw Exception: {ex.Message}");
                return null;
            }
        }
    }
}
