using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace BCMT_Associates.Models
{
    public class Roles
    {

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Access { get; set; }

         public ICollection<RoleMenu>? RoleMenu { get; set; }
    }



    public class Roless
    {
        //public Roles()
        //{
        //    Userss = new HashSet<Users>();
        //    RoleMenu = new HashSet<RoleMenu>();
        //}

        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Access { get; set; }
        public int[]? roleMenus { get; set; }

        //public ICollection<Users>? Userss { get; set; }
        public ICollection<RoleMenu>? RoleMenu { get; set; }
    }

}
