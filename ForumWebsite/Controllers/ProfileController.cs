using AutoMapper;
using ForumWebsite.Models.Authentication;
using ForumWebsite.Models.ViewMoels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumWebsite.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.Identity == null)
                return View();

            var result = await _userManager.FindByNameAsync(User.Identity.Name);
            if (result != null)
            {
                ProfileViewModel viewModel = new ProfileViewModel()
                {
                    Birthday = result.Birthday,
                    AboutMe = result.AboutMe
                };

                return View(viewModel);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProfileViewModel viewModel)
        {
            if (User.Identity == null)
                return Redirect("/User/AccessDenied");

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                if (user != null)
                {
                    user.Birthday = viewModel.Birthday;
                    user.AboutMe = viewModel.AboutMe;

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        await _userManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.SignInAsync(user, true);
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                    }

                    return View(viewModel);
                }
            }

            return View();
        }
    }
}
