using ForumWebsite.Models;
using ForumWebsite.Models.Authentication;
using ForumWebsite.Models.Context;
using ForumWebsite.Models.ViewMoels;
using Microsoft.EntityFrameworkCore;

namespace ForumWebsite.Services
{
    public interface IPostService
    {
        /// <summary>
        /// Gets the total number of posts.
        /// </summary>
        Task<int> GetAllPostCountAsync();

        /// <summary>
        /// Gets all the posts.
        /// </summary>
        Task<List<Post>> GetAllPostsAsync();

        /// <summary>
        /// Gets all the posts in a specific page.
        /// </summary>
        /// <param name="pageNumber">Current page number.</param>
        /// <param name="pageSize">The number of posts to show on the page.</param>
        Task<List<Post>> GetPostsPaginatedAsync(int pageNumber, int pageSize);

        /// <summary>
        /// Gets a specific post.
        /// </summary>
        /// <param name="id">The Id of the post</param>
        /// <returns>Returns null if no post is found.</returns>
        Task<Post?> GetPostAsync(long id);

        /// <summary>
        /// Creates a new post.
        /// </summary>
        /// <param name="user">The sender.</param>
        /// <param name="postData">The data coming from the view.</param>
        /// <returns>Returns the created entity.</returns>
        Task<Post?> CreatePostAsync(User user, NewPostViewModel postData);

        /// <summary>
        /// Edits an already exist post.
        /// </summary>
        /// <param name="id">The Id of the post that is going to be edited.</param>
        /// <param name="postData">The data coming from the view.</param>
        /// <returns>Returns true if the post is found and is edited successfully.</returns>
        Task<bool> EditPostAsync(long id, EditPostViewModel postData);

        /// <summary>
        /// Deletes a post from the database.
        /// </summary>
        /// <param name="id">The Id of the post that is going to be deleted.</param>
        /// <returns>Returns true if the post is found and is deleted successfully.</returns>
        Task<bool> DeletePostAsync(long id);

        /// <summary>
        /// Closes a post.
        /// </summary>
        /// <param name="id">The Id of the post that is goind to be closed.</param>
        /// <returns>Returns true if the post is found and is closed.</returns>
        Task<bool> ClosePostAsync(long id);
    }

    public class PostService : IPostService
    {
        private readonly IServiceProvider _serviceProvider;

        public PostService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> GetAllPostCountAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

                return await dbContext.Posts.CountAsync();
            }
        }

        public async Task<List<Post>> GetAllPostsAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

                return await dbContext.Posts.ToListAsync();
            }
        }

        public async Task<List<Post>> GetPostsPaginatedAsync(int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize <= 0)
                return new List<Post>();

            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

                int count = await GetAllPostCountAsync();
                return await dbContext.Posts.Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(x => x.User).ToListAsync();
            }
        }

        public async Task<Post?> GetPostAsync(long id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

                return await dbContext.Posts.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);
            }
        }

        public async Task<Post?> CreatePostAsync(User user, NewPostViewModel postData)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                var logger = scope.ServiceProvider.GetService<ILogger<CommentService>>();

                try
                {
                    var result = await dbContext.Posts.AddAsync(new Post()
                    {
                        Header = postData.Header,
                        Body = postData.Body,
                        CreatedTimestamp = DateTime.Now,
                        IsClosed = false,
                        UserId = user.Id
                    });

                    await dbContext.SaveChangesAsync();

                    logger?.LogInformation("--- A new post has been created: " + result.Entity.Id);
                    return result.Entity;
                }
                catch(Exception e)
                {
                    logger?.LogError("--- Something went wrong while creating a new post: " + e.Message);
                    return null;
                }
            }
        }

        public async Task<bool> EditPostAsync(long id, EditPostViewModel postData)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                var logger = scope.ServiceProvider.GetService<ILogger<CommentService>>();

                Post? searchResult = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
                if (searchResult != null)
                {
                    searchResult.Header = postData.Header;
                    searchResult.Body = postData.Body;

                    await dbContext.SaveChangesAsync();

                    logger?.LogInformation("--- A post has been edited: " + searchResult.Id);
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> DeletePostAsync(long id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                var logger = scope.ServiceProvider.GetService<ILogger<CommentService>>();

                Post? searchResult = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
                if (searchResult != null)
                {
                    dbContext.Posts.Remove(searchResult);
                    await dbContext.SaveChangesAsync();

                    logger?.LogInformation("--- A post has been deleted: " + searchResult.Id);
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> ClosePostAsync(long id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                var logger = scope.ServiceProvider.GetService<ILogger<CommentService>>();

                Post? searchResult = await dbContext.Posts.FirstOrDefaultAsync(x => x.Id == id);
                if (searchResult != null)
                {
                    searchResult.IsClosed = true;
                    await dbContext.SaveChangesAsync();

                    logger?.LogInformation("--- A post has been closed: " + searchResult.Id);
                    return true;
                }

                return false;
            }
        }
    }
}
