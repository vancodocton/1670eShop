using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Models
{
    public class ApplicationUser : IdentityUser<string>
    {
        public string FullName { get; set; }

        public string Address { get; set; }

        public ApplicationUser()
        {
            FullName = string.Empty;
            Address = string.Empty;
        }
    }
}
