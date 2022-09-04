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
    }
}
