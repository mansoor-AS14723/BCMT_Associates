using Microsoft.AspNetCore.Identity;

namespace BCMT_Associates.Models
{
    public class ApplicationUserToken : IdentityUserToken<int>
    {
        public int? Id { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}