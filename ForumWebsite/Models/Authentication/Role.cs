using Microsoft.AspNetCore.Identity;

namespace ForumWebsite.Models.Authentication
{
    public class Role : IdentityRole<string>
    {
        public DateTime CreatedTimestamp { get; set; }
    }
}
