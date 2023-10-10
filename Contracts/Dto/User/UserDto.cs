using Contracts.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.User
{
    public class UserDto
    {
        public string UserName { get; set; }
        public UserPermission UserPermission { get; set; }
        public IEnumerable<ModulePermissionContainer> ModulePermissions { get; set; }
    }
}
