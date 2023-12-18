namespace LibraryServices.Infrastructure.RedisCache
{
    public class RedisRequirement
    {
        public TimeSpan CacheTime { get; }

        public RedisRequirement(TimeSpan cacheTime)
        {
            CacheTime = cacheTime;
        }
    }
}
