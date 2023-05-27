using EngLift.Data.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Security.Claims;

namespace EngLift.Sercurity.Authorization
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        private IUnitOfWork _unitOfWork { set; get; }
        public RoleAuthorizationHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesAuthorizationRequirement requirement)
        {
            if (context.User == null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            if (requirement.AllowedRoles == null || requirement.AllowedRoles.Any() == false)
            {
                context.Succeed(requirement);
            }
            else
            {
                Guid userId = Guid.Parse(context.User.FindFirstValue("UserId"));
                bool found = _unitOfWork.UserRolesRepo.GetAll().Any(x => x.UserId == userId && requirement.AllowedRoles.Contains(x.Role.Name));
                if (found) context.Succeed(requirement);
                else context.Fail();
            }
            return Task.CompletedTask;

        }
    }
}
