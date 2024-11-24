using System;
using System.Collections.Generic;

namespace BCMT_Associates.Models
{
    public partial class RoleMenu
    {
        public int Id { get; set; }
        public int RolesId { get; set; }
        public int? ParentId { get; set; }
        public int MenusId { get; set; }
        public string? MenuName { get; set; }
        public string? Url { get; set; }
        public string? Icon { get; set; }
        public Menus Menus { get; set; }
        public Roles Roles { get; set; }
    }


    public partial class RoleMenus
    {
        public int Id { get; set; }
        public int RolesId { get; set; }
        public int MenusId { get; set; }
    }
}
