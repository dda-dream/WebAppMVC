using Microsoft.AspNetCore.Identity;

namespace WebAppMVC_Models
{ 
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
