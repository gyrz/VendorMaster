using Contracts.Dto.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Contracts.Dto.User;
using Newtonsoft.Json;

namespace VendorMaster.Extensions
{
    public class HasWritePermissionForModuleAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        readonly ModulePermission _requiredClaim;

        public HasWritePermissionForModuleAttribute(ModulePermission claim)
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

            var claim = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "ModulePermissions");
            if (claim == null)
            {
                context.Result = new ForbidResult();
                return;
            }

            var ownedClaims = JsonConvert.DeserializeObject<List<ModulePermissionContainer>>(claim.Value);
            var needed = ownedClaims.FirstOrDefault(x => x.ModulePermission == _requiredClaim);
            if (needed == null || needed.Write == false)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
