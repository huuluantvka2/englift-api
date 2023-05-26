using EngLift.Data;
using EngLift.Model.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace EngLift.Sercurity
{
    public static class IdentityFrameworkCoreExtension
    {
        public static IServiceCollection AddIdentityFrameworkCoreExtension(this IServiceCollection services)
        {
            services.AddIdentityCore<User>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<BuildDbContext>();
            return services;
        }
    }
}
