using EngLift.Common;
using EngLift.Data.Infrastructure.Factories;
using EngLift.Data.Infrastructure.Interfaces;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EngLift.Data.Infrastructure.GoogleEngine
{
    internal class ImportWordPublisher : IGooglePublisher
    {
        private TopicName _topicName = new TopicName(GoogleConstant.GoogleIdProject, GoogleConstant.PubSubTopicImport);
        private PublisherClient _publisher;
        private ILogger<GooglePublisherFactory> _logger;
        public ImportWordPublisher(ILogger<GooglePublisherFactory> logger)
        {
            _logger = logger;
        }

        public async Task SendMessageAsync<T>(T data)
        {
            try
            {
                _logger.LogInformation($"ImportWordPublisher -> SendMessageAsync");
                PublisherClient publisher = _publisher ?? (await PublisherClient.CreateAsync(_topicName));
                string messageId = await publisher.PublishAsync(JsonConvert.SerializeObject(data));
                await publisher.ShutdownAsync(TimeSpan.FromSeconds(15));
                _logger.LogInformation($"ImportWordPublisher -> SendMessageAsync successfully with messageId: {messageId}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"mportWordPublisher -> SendMessageAsync Throw Exception: {ex.Message}");
            }
        }
    }
}
