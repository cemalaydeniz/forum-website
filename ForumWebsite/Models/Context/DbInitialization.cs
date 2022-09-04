using ForumWebsite.Models.Authentication;
using ForumWebsite.Utility;
using Microsoft.AspNetCore.Identity;

namespace ForumWebsite.Models.Context
{
    public class DbInitialization
    {
        public async void InitializeRoles(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                var initialRoles = new[]
                {
                    new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = StringLibrary.RoleNames.Admin,
                        CreatedTimestamp = DateTime.Now
                    },
                    new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = StringLibrary.RoleNames.Moderator,
                        CreatedTimestamp = DateTime.Now
                    },
                    new Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = StringLibrary.RoleNames.Member,
                        CreatedTimestamp = DateTime.Now
                    }
                };

                foreach (var role in initialRoles)
                {
                    Role? existedRole = dbContext.Roles.FirstOrDefault(x => x.Name == role.Name);
                    if (existedRole == null)
                    {
                        await roleManager.CreateAsync(role);
                    }
                }
            }
        }
    }
}
