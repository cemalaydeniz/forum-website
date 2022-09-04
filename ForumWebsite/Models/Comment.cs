using ForumWebsite.Models.Authentication;

namespace ForumWebsite.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public string Body { get; set; } = null!;
        public DateTime CreatedTimestamp { get; set; }

        // Foreign keys
        public string UserId { get; set; } = null!;
        public long PostId { get; set; }

        // References
        public virtual User User { get; set; } = null!;
        public virtual Post Post { get; set; } = null!;
    }
}
