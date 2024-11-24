using Microsoft.AspNetCore.Identity;

namespace BCMT_Associates.Models
{
    public class ApplicationUserClaim : IdentityUserClaim<int>
    {
        public virtual ApplicationUser? User { get; set; }
    }
}