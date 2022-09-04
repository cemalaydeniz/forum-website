using System.ComponentModel.DataAnnotations;

namespace ForumWebsite.Models.ViewMoels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "Please enter your birthday.")]
        [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [DataType(DataType.Text)]
        [StringLength(255, ErrorMessage = "This section can be the maximum of 255 characters.")]
        [Display(Name = "About Me")]
        public string? AboutMe { get; set; }
    }
}
