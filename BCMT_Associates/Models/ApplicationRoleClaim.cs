using Microsoft.AspNetCore.Identity;

namespace BCMT_Associates.Models
{
    public class ApplicationRoleClaim : IdentityRoleClaim<int>
    {
        public virtual ApplicationRole? Role { get; set; }
    }
}