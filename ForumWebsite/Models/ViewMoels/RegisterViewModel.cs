using System.ComponentModel.DataAnnotations;

namespace ForumWebsite.Models.ViewMoels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please enter a user name.")]
        [StringLength(15, ErrorMessage = "The user name must be between 3 and 15 characters.", MinimumLength = 3)]
        [Display(Name = "User Name")]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = "Please enter an email address.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your birthday.")]
        [DataType(DataType.Date)]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Please enter a password.")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [StringLength(100, ErrorMessage = "The password must be at least 6 characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password, ErrorMessage = "Please enter a valid password.")]
        [Compare(nameof(Password), ErrorMessage = "The confirmation does not match with your password.")]
        [Display(Name = "Password Confirmation")]
        public string PasswordConfirm { get; set; } = null!;
    }
}
