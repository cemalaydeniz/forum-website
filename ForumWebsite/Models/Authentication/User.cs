using Microsoft.AspNetCore.Identity;

namespace ForumWebsite.Models.Authentication
{
    public class User : IdentityUser<string>
    {
        public User()
        {
            Posts = new HashSet<Post>();
            Comments = new HashSet<Comment>();
        }

        public DateTime Birthday { get; set; }

        // Relations
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
