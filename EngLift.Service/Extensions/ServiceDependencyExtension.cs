using EngLift.Service.Implements;
using EngLift.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace EngLift.Service.Extensions
{
    public static class ServiceDependencyExtension
    {
        public static IServiceCollection AddServiceDependencyExtension(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILessonService, LessonService>();
            services.AddScoped<IWordService, WordService>();

            return services;
        }
    }
}
