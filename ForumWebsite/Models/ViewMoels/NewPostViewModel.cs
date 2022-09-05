using System.ComponentModel.DataAnnotations;

namespace ForumWebsite.Models.ViewMoels
{
    public class NewPostViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "The header must be between 5 and 20 characters.", MinimumLength = 5)]
        [DataType(DataType.Text)]
        [Display(Name = "Header")]
        public string Header { get; set; } = null!;

        [Required]
        [StringLength(65535, ErrorMessage = "The body of the post must be between 10 and 65535 characters.", MinimumLength = 10)]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Body")]
        public string Body { get; set; } = null!;
    }
}
