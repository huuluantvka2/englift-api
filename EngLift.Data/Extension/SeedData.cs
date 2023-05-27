using EngLift.Common;
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

            #region Create roles
            List<Role> listRoles = new List<Role>()
            {
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_SYSTEM_ADMIN,Group=GROUP_ROLE.Admin},

                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_CAN_VIEW_COURSE,Group=GROUP_ROLE.Course},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_CREATE_VIEW_COURSE,Group=GROUP_ROLE.Course},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_EDIT_VIEW_COURSE,Group=GROUP_ROLE.Course},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_DELETE_VIEW_COURSE,Group=GROUP_ROLE.Course},

                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_CAN_VIEW_LESSON,Group=GROUP_ROLE.Lesson},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_CREATE_VIEW_LESSON,Group=GROUP_ROLE.Lesson},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_EDIT_VIEW_LESSON,Group=GROUP_ROLE.Lesson},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_DELETE_VIEW_LESSON,Group=GROUP_ROLE.Lesson},

                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_CAN_VIEW_WORD,Group=GROUP_ROLE.Word},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_CREATE_VIEW_WORD,Group=GROUP_ROLE.Word},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_EDIT_VIEW_WORD,Group=GROUP_ROLE.Word},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_DELETE_VIEW_WORD,Group=GROUP_ROLE.Word},

                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_CAN_VIEW_USER,Group=GROUP_ROLE.User},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_CREATE_VIEW_USER,Group=GROUP_ROLE.User},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_EDIT_VIEW_USER,Group=GROUP_ROLE.User},
                new Role(){Id = Guid.NewGuid(),Name=RolesName.ROLE_USER_DELETE_VIEW_USER,Group=GROUP_ROLE.User},
            };
            var dbRoles = context.Roles.ToList();
            foreach (var role in listRoles)
            {
                if (!dbRoles.Any(x => x.Name == role.Name)) context.Roles.Add(role);
            }
            context.SaveChanges();
            #endregion
            #region Create user
            Guid? userId = context.Users.Where(x => x.Email == "admin@gmail.com").Select(x => x.Id).FirstOrDefault();
            if (userId == Guid.Empty)
            {
                User admin = new User()
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
            #endregion

            #region UserRole
            Guid? adminId = context.Users.Where(x => x.Email == "admin@gmail.com").Select(x => x.Id).FirstOrDefault();
            if (adminId != Guid.Empty)
            {
                UserRole userRole = context.UserRoles.Where(x => x.UserId == adminId && x.Role != null ? x.Role.Name == RolesName.ROLE_SYSTEM_ADMIN : false).FirstOrDefault();
                if (userRole == null)
                {
                    context.UserRoles.Add(new UserRole()
                    {
                        UserId = (Guid)adminId,
                        RoleId = context.Roles.Where(x => x.Name == RolesName.ROLE_SYSTEM_ADMIN).Select(x => x.Id).First(),
                    });
                    context.SaveChanges();
                }
            }
            #endregion
        }
    }
}
