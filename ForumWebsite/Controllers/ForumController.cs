using ForumWebsite.Models;
using ForumWebsite.Models.Authentication;
using ForumWebsite.Models.ViewMoels;
using ForumWebsite.Services;
using ForumWebsite.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForumWebsite.Controllers
{
    public class ForumController : Controller
    {
        private static readonly int PAGE_SIZE = 10;

        private readonly UserManager<User> _userManager;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        public ForumController(UserManager<User> userManager, IPostService postService, ICommentService commentService)
        {
            _userManager = userManager;
            _postService = postService;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery]int page)
        {
            List<Post> posts = await _postService.GetPostsPaginatedAsync(page, PAGE_SIZE);

            ViewBag.Page = page;
            return View(posts);
        }

        [Authorize]
        [HttpGet]
        public IActionResult NewPost()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> NewPost(NewPostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(User.Identity?.Name);
                if (user != null)
                {
                    Post? result = await _postService.CreatePostAsync(user, viewModel);
                    if (result != null)
                    {
                        return RedirectToAction("PostView", new { postId = result.Id });
                    }

                    ModelState.AddModelError(StringLibrary.PostErrors.NewPostFailCode, StringLibrary.PostErrors.NewPostFailDescription);
                }
                else
                {
                    ModelState.AddModelError(StringLibrary.PostErrors.UserNotFoundCode, StringLibrary.PostErrors.UserNotFoundDescription);
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PostView([FromQuery]long postId)
        {
            Post? post = await _postService.GetPostAsync(postId);
            if (post != null)
            {
                return View(new PostViewModel()
                {
                    Post = post,
                    Comments = await _commentService.GetAllCommentsAsync(postId)
                });
            }

            ModelState.AddModelError(StringLibrary.PostErrors.PostNotFoundCode, StringLibrary.PostErrors.PostNotFoundDescription);
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostView([FromQuery]long postId, PostViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(User.Identity?.Name);
                if (user != null)
                {
                    Comment? comment = await _commentService.CreateCommentAsync(user, postId, viewModel);
                    if (comment != null)
                    {
                        return RedirectToAction("PostView", new { postId = postId });
                    }

                    ModelState.AddModelError(StringLibrary.PostErrors.NewCommentFailCode, StringLibrary.PostErrors.NewCommentFailDescription);
                }
                else
                {
                    ModelState.AddModelError(StringLibrary.PostErrors.UserNotFoundCode, StringLibrary.PostErrors.UserNotFoundDescription);
                }
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditPost([FromQuery]long postId)
        {
            if (User.Identity == null)
                return Redirect("/User/AccessDenied");

            User user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user != null)
            {
                Post? post = await _postService.GetPostAsync(postId);
                if (post != null)
                {
                    if (post.UserId != user.Id)
                        return Redirect("/User/AccessDenied");

                    return View(new EditPostViewModel()
                    {
                        PostId = postId,
                        Header = post.Header,
                        Body = post.Body
                    });
                }
                else
                {
                    ModelState.AddModelError(StringLibrary.PostErrors.PostNotFoundCode, StringLibrary.PostErrors.PostNotFoundDescription);
                }
            }
            else
            {
                ModelState.AddModelError(StringLibrary.PostErrors.UserNotFoundCode, StringLibrary.PostErrors.UserNotFoundDescription);
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPost([FromQuery]long postId, EditPostViewModel viewModel)
        {
            if (User.Identity == null)
                return Redirect("/User/AccessDenied");

            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(User.Identity?.Name);
                if (user != null)
                {
                    Post? post = await _postService.GetPostAsync(postId);
                    if (post != null)
                    {
                        if (post.UserId != user.Id)
                            return Redirect("/User/AccessDenied");

                        if (await _postService.EditPostAsync(postId, viewModel))
                        {
                            return RedirectToAction("PostView", new { postId = postId });
                        }
                        else
                        {
                            ModelState.AddModelError(StringLibrary.PostErrors.EditPostFailCode, StringLibrary.PostErrors.EditPostFailDescription);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(StringLibrary.PostErrors.PostNotFoundCode, StringLibrary.PostErrors.PostNotFoundDescription);
                    }
                }
                else
                {
                    ModelState.AddModelError(StringLibrary.PostErrors.UserNotFoundCode, StringLibrary.PostErrors.UserNotFoundDescription);
                }
            }

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditComment([FromQuery]long commentId)
        {
            if (User.Identity == null)
                return Redirect("/User/AccessDenied");

            User user = await _userManager.FindByNameAsync(User.Identity?.Name);
            if (user != null)
            {
                Comment? comment = await _commentService.GetCommentAsync(commentId);
                if (comment != null)
                {
                    if (comment.UserId != user.Id)
                        return Redirect("/User/AccessDenied");

                    return View(new EditCommentViewModel()
                    {
                        CommentId = commentId,
                        Body = comment.Body
                    });
                }
                else
                {
                    ModelState.AddModelError(StringLibrary.PostErrors.CommentNotFoundCode, StringLibrary.PostErrors.CommentNotFoundDescription);
                }
            }
            else
            {
                ModelState.AddModelError(StringLibrary.PostErrors.UserNotFoundCode, StringLibrary.PostErrors.UserNotFoundDescription);
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditComment([FromQuery]long commentId, EditCommentViewModel viewModel)
        {
            if (User.Identity == null)
                return Redirect("/User/AccessDenied");

            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(User.Identity?.Name);
                if (user != null)
                {
                    Comment? comment = await _commentService.GetCommentAsync(commentId);
                    if (comment != null)
                    {
                        if (comment.UserId != user.Id)
                            return Redirect("/User/AccessDenied");

                        if (await _commentService.EditCommentAsync(commentId, viewModel))
                        {
                            return RedirectToAction("PostView", new { postId = comment.PostId });
                        }
                        else
                        {
                            ModelState.AddModelError(StringLibrary.PostErrors.EditPostFailCode, StringLibrary.PostErrors.EditPostFailDescription);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(StringLibrary.PostErrors.PostNotFoundCode, StringLibrary.PostErrors.PostNotFoundDescription);
                    }
                }
                else
                {
                    ModelState.AddModelError(StringLibrary.PostErrors.UserNotFoundCode, StringLibrary.PostErrors.UserNotFoundDescription);
                }
            }

            return View();
        }
    }
}
