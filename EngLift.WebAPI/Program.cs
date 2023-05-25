using EngLift.Data;
using EngLift.Data.Extension;
using EngLift.DTO.User;
using EngLift.Service.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
var app = builder.Build();

#region Seed Data
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
