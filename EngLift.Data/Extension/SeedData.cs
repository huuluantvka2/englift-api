using EngLift.DTO.User;
using EngLift.Model.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EngLift.Data.Extension
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, UserAdminSeedDTO adminDTO)
        {
            var context = new BuildDbContext(serviceProvider.GetRequiredService<DbContextOptions<BuildDbContext>>(), null);

            if (context.Users.Any(x => x.Email == "admin@gmail.com"))
            {
                return;
            }


            var admin = new User()
            {
                IsAdmin = true,
                Id = Guid.NewGuid(),
                Email = adminDTO.Email,
                EmailConfirmed = true,
                NormalizedEmail = adminDTO.Email,
                NormalizedUserName = adminDTO.UserName,
                UserName = adminDTO.UserName,
                PhoneNumber = "0339181198",
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                FullName = "Admin"
            };

            var passwordHasher = new PasswordHasher<User>();
            admin.PasswordHash = passwordHasher.HashPassword(admin, adminDTO.Password);


            context.Users.Add(admin);
            context.SaveChanges();
        }
    }
}
