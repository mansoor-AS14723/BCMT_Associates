using Microsoft.AspNetCore.Identity;

namespace BCMT_Associates.Models
{
    public class ApplicationUserRole : IdentityUserRole<int>
    {
        public int? Id { get; set; }
        internal ApplicationUser? User { get; set; }

        internal ApplicationRole? Role { get; set; }
    }
}