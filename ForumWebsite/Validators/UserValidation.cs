using ForumWebsite.Models.Authentication;
using Microsoft.AspNetCore.Identity;

namespace ForumWebsite.Validators
{
    public class UserValidation : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (int.TryParse(user.UserName[0].ToString(), out int _))
            {
                errors.Add(new IdentityError()
                {
                    Code = "UserNameStartsWithNumber",
                    Description = "The user name cannot start with a number"
                });
            }

            return errors.Count == 0 ? Task.FromResult(IdentityResult.Success) : Task.FromResult(IdentityResult.Failed(errors.ToArray()));
        }
    }
}
