using AutoMapper;
using ForumWebsite.Models.Authentication;
using ForumWebsite.Models.ViewMoels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<User> userManager, IMapper mapper, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User newUser = _mapper.Map<User>(viewModel);
                newUser.Id = Guid.NewGuid().ToString();

                var result = await _userManager.CreateAsync(newUser, viewModel.Password);
                if (result.Succeeded)
                {
                    // TODO: Add roles and other things here

                    _logger.LogInformation("--- A new user is registered: " + newUser.UserName);

                    return RedirectToAction("RegisterSuccess");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult RegisterSuccess()
        {
            return View();
        }
    }
}
