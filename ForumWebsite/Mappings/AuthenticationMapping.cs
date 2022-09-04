using AutoMapper;
using ForumWebsite.Models.Authentication;
using ForumWebsite.Models.ViewMoels;

namespace ForumWebsite.Mappings
{
    public class AuthenticationMapping : Profile
    {
        public AuthenticationMapping()
        {
            CreateMap<User, RegisterViewModel>();
            CreateMap<RegisterViewModel, User>();
        }
    }
}
