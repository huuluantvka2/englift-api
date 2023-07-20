using EngLift.Data;
using EngLift.Data.Extension;
using EngLift.DTO.User;
using EngLift.Sercurity;
using EngLift.Service.Extensions;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", builder.Configuration["environmentVariables:GOOGLE_APPLICATION_CREDENTIALS"]);
var services = builder.Services;
services.AddControllers();
services.AddEndpointsApiExplorer();

#region MYSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Connection {connectionString}");
services.AddDbContext<BuildDbContext>(options =>
{
    /*    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));*/
    options.UseSqlServer(connectionString);
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
services.AddIdentityFrameworkCore();
services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        ValidAudience = builder.Configuration["JWT:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Key),
        ClockSkew = TimeSpan.Zero
    };
});

services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Eng Word Sync API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer",
                }
            },
            new string[]{}
        }
    });
});
#endregion

#region Cors
services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificDomain",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "https://localhost:3000", "https://tienganhtot.portal.canbantot.com", "https://learn.canbantot.com")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
#endregion

#region Firebase Init
FirebaseApp.Create(new AppOptions()
{
    Credential = GoogleCredential.FromFile("./englift-b241a-firebase-adminsdk.json"),
});
#endregion
#region Worker Service
services.AddHostedService<ImportWordWorker.ImportWordWorker>();
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
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}*/

app.UseStaticFiles();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("v1/swagger.json", "Englift V1");
});
app.UseCors("AllowSpecificDomain");
//global handle error
app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
