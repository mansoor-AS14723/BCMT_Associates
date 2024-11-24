using Microsoft.AspNetCore.Identity;

namespace BCMT_Associates.Models
{
    public class ApplicationRole : IdentityRole<int>
    {
        public string? Access { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

        public ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    }
}
