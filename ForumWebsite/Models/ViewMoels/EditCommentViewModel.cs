using System.ComponentModel.DataAnnotations;

namespace ForumWebsite.Models.ViewMoels
{
    public class EditCommentViewModel
    {
        public long CommentId { get; set; }

        [Required]
        [StringLength(65535, ErrorMessage = "The comment must be between 10 and 65535 characters.", MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Body")]
        public string Body { get; set; } = null!;
    }
}
