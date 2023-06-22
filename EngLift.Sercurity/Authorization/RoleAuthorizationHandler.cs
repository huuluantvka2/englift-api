using EngLift.Common;
using EngLift.Data.Infrastructure.Interfaces;
using EngLift.Service.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Net;
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
                throw new ServiceExeption(HttpStatusCode.Unauthorized, ErrorMessage.UNAUTHORIZED);
            }

            if (requirement.AllowedRoles == null || requirement.AllowedRoles.Any() == false)
            {
                context.Succeed(requirement);
            }
            else
            {
                Guid userId = Guid.Parse(context.User.FindFirstValue("UserId"));
                var roles = context.User.FindFirst(ClaimTypes.Role).Value;
                bool found = requirement.AllowedRoles.Any(x => roles.Contains(x));
                //bool found = _unitOfWork.UserRolesRepo.GetAll().Any(x => x.UserId == userId && requirement.AllowedRoles.Any(y => y == x.Role.Name));
                if (found) context.Succeed(requirement);
                else throw new ServiceExeption(HttpStatusCode.Forbidden, ErrorMessage.FORBIDDEN);
            }
            return Task.CompletedTask;

        }
    }
}
