using EngLift.Data;
using EngLift.Data.Extension;
using EngLift.DTO.User;
using EngLift.Sercurity;
using EngLift.Service.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

#region MYSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
services.AddDbContext<BuildDbContext>(options =>
{
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
#endregion

#region Dependency Injection
services.AddHttpContextAccessor();
services.AddRepositoryDependencyExtensition();
services.AddServiceDependencyExtension();
#endregion

#region Logging
string outputFormat = "{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";
string[] excludedKeywords = { "Route matched with", "Executing JsonResult", "Executed action", "Executed endpoint", "Executing endpoint" };
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Filter.ByExcluding(logEvent => excludedKeywords.Any(keyword => logEvent.MessageTemplate.Text.Contains(keyword)))
    .WriteTo.Console(outputTemplate: outputFormat)
    .WriteTo.RollingFile("Logs\\Log-{Date}.txt", retainedFileCountLimit: null, outputTemplate: outputFormat)
    .CreateLogger();
services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog();
});
#endregion
#region Identity
services.AddIdentityFrameworkCoreExtension();
#endregion

var app = builder.Build();

#region Seed Data
if (bool.Parse(builder.Configuration["SeedData"]) == true)
{
    using (var scope = app.Services.CreateScope())
    {
        var adminDTO = new UserAdminSeedDTO()
        {
            Email = builder.Configuration["AdminAccount:Email"],
            UserName = builder.Configuration["AdminAccount:UserName"],
            Password = builder.Configuration["AdminAccount:Password"],
        };
        SeedData.Initialize(scope.ServiceProvider, adminDTO);
    }
}
#endregion


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
