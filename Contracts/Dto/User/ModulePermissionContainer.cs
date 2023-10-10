using Contracts.Dto.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Dto.User
{
    public class ModulePermissionContainer
    {
        public ModulePermission ModulePermission { get; set; }
        public bool Write { get; set; }
    }
}
