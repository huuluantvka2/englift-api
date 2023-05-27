using EngLift.Data.Infrastructure.Implements;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EngLift.Data.Extension
{
    public static class RepositoryDependencyExtensition
    {
        public static IServiceCollection AddRepositoryDependencyExtensition(this IServiceCollection services)
        {
            services.AddScoped<IDbFactory, DbFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<ILessonRepository, LessonRepository>();

            return services;
        }
    }
}
