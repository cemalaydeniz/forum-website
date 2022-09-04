using System.ComponentModel.DataAnnotations;

namespace ForumWebsite.Models.ViewMoels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(100, ErrorMessage = "The password must be at least 6 characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember Me?")]
        public bool RememberMe { get; set; }
    }
}
