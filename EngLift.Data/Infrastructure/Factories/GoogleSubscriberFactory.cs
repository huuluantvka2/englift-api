using EngLift.Common;
using EngLift.Data.Infrastructure.GoogleEngine;
using EngLift.Data.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace EngLift.Data.Infrastructure.Factories
{
    public class GoogleSubscriberFactory : IGoogleSucscriberFactory
    {
        private ILogger<GoogleSubscriberFactory> _logger;
        private IGoogleSubscriber _importWord;
        public GoogleSubscriberFactory(ILogger<GoogleSubscriberFactory> logger)
        {
            _logger = logger;
        }
        public IGoogleSubscriber Init(string subscriptionName)
        {
            if (subscriptionName == GoogleConstant.PubSubSubscriptionImport)
            {
                return (_importWord ?? (_importWord = new ImportWordSubscriber(_logger)));
            }
            return null;
        }
    }
}
