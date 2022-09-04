using ForumWebsite.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ForumWebsite.Validators
{
    public class PasswordValidation : IPasswordValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError()
                {
                    Code = "PasswordContainsUserName",
                    Description = "The password cannot contain your user name."
                });
            }

            return errors.Count == 0 ? Task.FromResult(IdentityResult.Success) : Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}
