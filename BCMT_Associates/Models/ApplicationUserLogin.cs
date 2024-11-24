using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BCMT_Associates.Models
{
    
    public class ApplicationUserLogin : IdentityUserLogin<int>
    {
        public int? Id { get; set; }
        public virtual ApplicationUser? User { get; set; }
    }
}