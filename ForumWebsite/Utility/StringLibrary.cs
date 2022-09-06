namespace ForumWebsite.Utility
{
    public static class StringLibrary
    {
        public static class RoleNames
        {
            public static readonly string Admin = "Admin";
            public static readonly string Moderator = "Moderator";
            public static readonly string Member = "Member";
        }

        public static class LoginErrors
        {
            public static readonly string AccountLockedCode = "AccountLocked";
            public static readonly string AccountLockedDescription = "The account is locked for 5 minutes due to the 5 failed login attempts.";

            public static readonly string UserNotFoundCode = "UserNotFound";
            public static readonly string UserNotFoundDescription = "The email address could not be found.";
        }

        public static class PostErrors
        {
            public static readonly string UserNotFoundCode = "UserNotFound";
            public static readonly string UserNotFoundDescription = "You must log in to create a new post.";

            public static readonly string NewPostFailCode = "NewPostFail";
            public static readonly string NewPostFailDescription = "The post could not be created. Please try again later.";

            public static readonly string PostNotFoundCode = "PostNotFound";
            public static readonly string PostNotFoundDescription = "The post could not be found.";

            public static readonly string NewCommentFailCode = "NewCommentFail";
            public static readonly string NewCommentFailDescription = "The comment could not be created. Please try again later.";

            public static readonly string EditPostFailCode = "EditPostFail";
            public static readonly string EditPostFailDescription = "The post could not be edited. Please try again later.";

            public static readonly string CommentNotFoundCode = "CommentNotFound";
            public static readonly string CommentNotFoundDescription = "The comment could not be found.";

            public static readonly string DeletePostRoleFailCode = "DeletePostRoleFail";
            public static readonly string DeletePostRoleFailDescription = "You do not have a permission to delete a post.";

            public static readonly string CloseRoleFailCode = "CloseRoleFail";
            public static readonly string CloseRoleFailDescription = "You do not have a permission to close a post.";

            public static readonly string DeleteCommentRoleFailCode = "DeleteCommentRoleFail";
            public static readonly string DeleteCommentRoleFailDescription = "You do not have a permission to delete a comment.";
        }
    }
}
