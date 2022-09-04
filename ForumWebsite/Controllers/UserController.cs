using AutoMapper;
using ForumWebsite.Models.Authentication;
using ForumWebsite.Models.ViewMoels;
using ForumWebsite.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumWebsite.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                    await _userManager.AddToRoleAsync(newUser, StringLibrary.RoleNames.Member);

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

        [HttpGet]
        public IActionResult Login(string? returnUrl)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(viewModel.Email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var result = await _signInManager.PasswordSignInAsync(user, viewModel.Password, viewModel.RememberMe, true);
                    if (result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(user);

                        if (string.IsNullOrEmpty(TempData["ReturnUrl"] != null ? TempData["ReturnUrl"].ToString() : ""))
                            return Redirect("/Home/Index");

                        return Redirect(TempData["ReturnUrl"].ToString());
                    }
                    else
                    {
                        if (result.IsLockedOut)
                        {
                            ModelState.AddModelError(StringLibrary.LoginErrors.AccountLockedCode, StringLibrary.LoginErrors.AccountLockedDescription);
                        }
                        else
                        {
                            int failCount = await _userManager.GetAccessFailedCountAsync(user);
                            if (failCount == 5)
                            {
                                await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(5)));

                                ModelState.AddModelError(StringLibrary.LoginErrors.AccountLockedCode, StringLibrary.LoginErrors.AccountLockedDescription);
                            }
                            else
                            {
                                ModelState.AddModelError(StringLibrary.LoginErrors.UserNotFoundCode, StringLibrary.LoginErrors.UserNotFoundDescription);
                            }
                        }
                    }
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("LogoutSuccess");
        }

        [HttpGet]
        public IActionResult LogoutSuccess()
        {
            Response.Headers.Add("REFRESH", "5;/Home/Index");
            return View();
        }
    }
}
