using Contracts.Dto.Enums;
using Contracts.Dto.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicService.User.UserTemplate
{
    public class AdminUser : UserTemplate
    {
        public AdminUser()
        {
            UserDto = new UserDto
            {
                UserName = "admin",
                UserPermission = UserPermission.SA,
                ModulePermissions = new List<ModulePermission>
                {
                    ModulePermission.BASE,
                    ModulePermission.VENDORMASTER
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
                ModulePermissions = new List<ModulePermission>
                    {
                        ModulePermission.BASE
                    }
            };
        }
    }
}
