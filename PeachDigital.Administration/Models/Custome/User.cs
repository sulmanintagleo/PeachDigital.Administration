using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PeachDigital.Administration.Models.Custome
{
    [Serializable]
    public partial class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public string County { get; set; }
        public string Zip { get; set; }
        public bool isActive { get; set; }
        public int RoleId { get; set; }
        
        public Role Role { get; set; }
        
    }

    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ModulePermissions> Permissions { get; set; }
    }
}