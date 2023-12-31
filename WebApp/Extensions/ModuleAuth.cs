﻿using Contracts.Dto.Enums;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using Contracts.Dto.User;

namespace VendorMaster.Extensions
{
    public class ModuleAuthAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        readonly ModulePermission[] _requiredClaims;

        public ModuleAuthAttribute(params ModulePermission[] claims)
        {
            _requiredClaims = claims;
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
            var ownedPermissions = ownedClaims.Select(x => x.ModulePermission).ToList();

            if (_requiredClaims.Where(x => !ownedPermissions.Contains(x)).Count() != 0)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
