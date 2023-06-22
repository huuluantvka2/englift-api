using EngLift.Common;
using EngLift.Data.Infrastructure.GoogleEngine;
using EngLift.Data.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace EngLift.Data.Infrastructure.Factories
{
    public class GooglePublisherFactory : IGooglePublisherFactory
    {
        private ILogger<GooglePublisherFactory> _logger;
        private IGooglePublisher _importWord;
        public GooglePublisherFactory(ILogger<GooglePublisherFactory> logger)
        {
            _logger = logger;
        }
        public IGooglePublisher Init(string topicName)
        {
            if (topicName == GoogleConstant.PubSubTopicImport)
            {
                return (_importWord ?? (_importWord = new ImportWordPublisher(_logger)));
            }
            return null;
        }
    }
}
