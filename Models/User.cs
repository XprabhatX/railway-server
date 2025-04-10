using Microsoft.AspNetCore.Identity;

namespace Authentication.Models
{
    public class User : IdentityUser
    {
        public string AadharNumber {get; set;}
    }
}