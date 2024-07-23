using Microsoft.AspNetCore.Authorization;
using Spa.Domain.Entities;
using Spa.Domain.Exceptions;
using System.Security.Claims;

namespace Spa.Application.Authorize.Authorization
{
    public class PermissionAuthorizationHandler:AuthorizationHandler<PermissionRequirment>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirment requirement)
        {
            var permissions = context.User.Claims.Where(c => c.Type == GlobalConstant.ClaimCustoms.Permissions)
                        .Select(c => c.Value)
                        .ToList();
            if (permissions.Contains(requirement.Permission))
                context.Succeed(requirement);
            else
                throw new InsufficientPermissionsException("Bạn không có quyền thực hiện hành động này.");
            return Task.CompletedTask;
        }
    }
}
