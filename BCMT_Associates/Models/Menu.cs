using System;
using System.Collections.Generic;

namespace BCMT_Associates.Models
{
    public partial class Menus
    {
        public Menus()
        {
            RoleMenu = new HashSet<RoleMenu>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }

        public ICollection<RoleMenu> RoleMenu { get; set; }
    }


    public partial class Menu
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Url { get; set; }
        public int ParentId { get; set; }

    }
}
