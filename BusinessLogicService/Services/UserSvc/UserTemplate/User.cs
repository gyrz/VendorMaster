using Contracts.Dto.Enums;
using Contracts.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.Services.UserSvc.UserTemplate
{
    public class AdminUser : UserTemplate
    {
        public AdminUser()
        {
            UserDto = new UserDto
            {
                UserName = "admin",
                UserPermission = UserPermission.SA,
                ModulePermissions = new List<ModulePermissionContainer>
                {
                    new ModulePermissionContainer() { ModulePermission = ModulePermission.BASE, Write = true },
                    new ModulePermissionContainer() { ModulePermission = ModulePermission.VENDORMASTER, Write = true },
                }
            };
        }
    }

    public class BaseUser : UserTemplate
    {
        public BaseUser()
        {
            UserDto = new UserDto
            {
                UserName = "user",
                UserPermission = UserPermission.USR,
                ModulePermissions = new List<ModulePermissionContainer>
                {
                    new ModulePermissionContainer() { ModulePermission = ModulePermission.BASE, Write = false },
                    new ModulePermissionContainer() { ModulePermission = ModulePermission.VENDORMASTER, Write = true },
                }
            };
        }
    }
}
