IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ImportWordWorker.ImportWordWorker>();
    })
    .Build();

await host.RunAsync();
