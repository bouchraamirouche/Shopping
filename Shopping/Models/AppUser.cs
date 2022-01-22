using Microsoft.AspNetCore.Identity;

namespace Shopping.Models
{
    public class AppUser : IdentityUser
    {
      
            public string Occupation { get; set; }
      
    }
}
