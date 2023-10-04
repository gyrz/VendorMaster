using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Contracts.Dto.Enums;

namespace VendorMaster.Extensions
{
    public class RoleAuthAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        readonly UserPermission _requiredClaim;

        public RoleAuthAttribute(UserPermission claim)
        {
            _requiredClaim = claim;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;
            if (!isAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (claim == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            Enum.TryParse<UserPermission>(claim.Value, out UserPermission current);
            if (_requiredClaim > current)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
