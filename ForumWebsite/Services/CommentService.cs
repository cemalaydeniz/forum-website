using ForumWebsite.Models;
using ForumWebsite.Models.Authentication;
using ForumWebsite.Models.Context;
using ForumWebsite.Models.ViewMoels;
using Microsoft.EntityFrameworkCore;

namespace ForumWebsite.Services
{
    public interface ICommentService
    {
        /// <summary>
        /// Gets the total number of comments in a post.
        /// </summary>
        /// <param name="postId">The Id of the post.</param>
        Task<int> GetAllCommentsCountAsync(long postId);

        /// <summary>
        /// Gets all the comments in a post.
        /// </summary>
        /// <param name="postId">The Id of the post.</param>
        Task<List<Comment>> GetAllCommentsAsync(long postId);

        /// <summary>
        /// Gets a specific comment.
        /// </summary>
        /// <param name="commentId">The Id of the comment.</param>
        Task<Comment?> GetCommentAsync(long commentId);

        /// <summary>
        /// Creates a new comment on a post.
        /// </summary>
        /// <param name="user">The sender</param>
        /// <param name="postId">The Id of the post that is going to be commented.</param>
        /// <param name="commentData">The data coming from the view.</param>
        /// <returns>Returns the created comment entity.</returns>
        Task<Comment?> CreateCommentAsync(User user, long postId, PostViewModel commentData);

        /// <summary>
        /// Edits an already exist comment.
        /// </summary>
        /// <param name="id">The Id of the comment that is going to be edited.</param>
        /// <param name="commentData">The data coming from the view.</param>
        /// <returns>Returns true if the comment is found and is edited.</returns>
        Task<bool> EditCommentAsync(long id, EditCommentViewModel commentData);

        /// <summary>
        /// Deletes a comment from the database.
        /// </summary>
        /// <param name="id">The Id of the comment that is going to be deleted./param>
        /// <returns>Returns true if the comment is found and is deleted.</returns>
        Task<bool> DeleteCommentAsync(long id);
    }

    public class CommentService : ICommentService
    {
        private readonly IServiceProvider _serviceProvider;

        public CommentService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<int> GetAllCommentsCountAsync(long postId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

                return await dbContext.Comments.Where(x => x.PostId == postId).CountAsync();
            }
        }

        public async Task<List<Comment>> GetAllCommentsAsync(long postId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

                return await dbContext.Comments.Where(x => x.PostId == postId).Include(x => x.User).ToListAsync();
            }
        }

        public async Task<Comment?> GetCommentAsync(long commentId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();

                return await dbContext.Comments.Include(x => x.User).FirstOrDefaultAsync(x => x.Id == commentId);
            }
        }

        public async Task<Comment?> CreateCommentAsync(User user, long postId, PostViewModel commentData)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                var logger = scope.ServiceProvider.GetService<ILogger<CommentService>>();

                try
                {
                    var result = await dbContext.Comments.AddAsync(new Comment()
                    {
                        Body = commentData.CommentBody,
                        CreatedTimestamp = DateTime.Now,
                        UserId = user.Id,
                        PostId = postId
                    });

                    await dbContext.SaveChangesAsync();

                    logger?.LogInformation("--- A new comment has been created: " + result.Entity.Id);
                    return result.Entity;
                }
                catch (Exception e)
                {
                    logger?.LogError("--- Something went wrong while creating a new comment: " + e.Message);
                    return null;
                }
            }
        }

        public async Task<bool> EditCommentAsync(long id, EditCommentViewModel commentData)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                var logger = scope.ServiceProvider.GetService<ILogger<CommentService>>();

                Comment? searchResult = await dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
                if (searchResult != null)
                {
                    searchResult.Body = commentData.Body;

                    await dbContext.SaveChangesAsync();

                    logger?.LogInformation("--- A comment has been edited: " + searchResult.Id);
                    return true;
                }

                return false;
            }
        }

        public async Task<bool> DeleteCommentAsync(long id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ForumDbContext>();
                var logger = scope.ServiceProvider.GetService<ILogger<CommentService>>();

                Comment? searchResult = await dbContext.Comments.FirstOrDefaultAsync(x => x.Id == id);
                if (searchResult != null)
                {
                    dbContext.Comments.Remove(searchResult);
                    await dbContext.SaveChangesAsync();

                    logger?.LogInformation("--- A comment has been deleted: " + searchResult.Id);
                    return true;
                }

                return false;
            }
        }
    }
}
