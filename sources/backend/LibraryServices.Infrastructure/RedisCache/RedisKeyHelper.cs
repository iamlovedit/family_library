namespace LibraryServices.Infrastructure.RedisCache
{
    public static class RedisKeyHelper
    {
        public static string GetUserByUsernameKey(string username)
        {
            return $"identity/user?username={username}";
        }

        public static string GetUserByIdKey(long userId)
        {
            return $"identity/user?id={userId}";
        }
    }
}
