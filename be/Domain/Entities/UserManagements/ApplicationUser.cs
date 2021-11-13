using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.UserManagements
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile UserProfile { get; set; }
    }
}