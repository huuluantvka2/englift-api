using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.DTO.Word;
using EngLift.Service.Interfaces;

namespace ImportWordWorker
{
    public class ImportWordWorker : BackgroundService
    {
        private readonly ILogger<ImportWordWorker> _logger;
        private IGoogleSubscriber _subscriber;
        private IServiceProvider _serviceProvider;
        private IDictionaryService _dictionaryService;
        public ImportWordWorker(ILogger<ImportWordWorker> logger, IGoogleSucscriberFactory factory, IServiceProvider serviceProvider, IDictionaryService dictionaryService)
        {
            _logger = logger;
            _subscriber = factory.Init(GoogleConstant.PubSubSubscriptionImport);
            _serviceProvider = serviceProvider;
            _dictionaryService = dictionaryService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    IWorkerService workerService = scope.ServiceProvider.GetRequiredService<IWorkerService>();
                    var list = await _subscriber.ProcessMessage<WordCreateExcelDTO>();
                    await workerService.SaveWordsAsync(list);

                }
            }
        }
    }
}
