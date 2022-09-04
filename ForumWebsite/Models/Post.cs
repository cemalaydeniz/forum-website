using ForumWebsite.Models.Authentication;

namespace ForumWebsite.Models
{
    public class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public long Id { get; set; }
        public string Header { get; set; } = null!;
        public string Body { get; set; } = null!;
        public DateTime CreatedTimestamp { get; set; }
        public bool IsClosed { get; set; }

        // Foreign keys
        public string UserId { get; set; } = null!;

        // References
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; } = null!;
    }
}
