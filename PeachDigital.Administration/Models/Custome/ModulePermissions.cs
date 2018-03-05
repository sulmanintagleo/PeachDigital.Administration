using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeachDigital.Administration.Models.Custome
{
    public class ModulePermissions
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public Int16 PermissionId { get; set; }
        public string PermissionaName { get; set; }
    }
}