using StackExchange.Redis;

namespace LibraryServices.Infrastructure.Lock;

public interface IRedisLock
{
    Task<IRedisLock> CreateLockAsync(IDatabase database, string lockKey, string lockValue, TimeSpan timeSpan);

    bool IsAcquired();

    void Release();
}

public class RedisLock : IRedisLock
{
    public Task<IRedisLock> CreateLockAsync(IDatabase database, string lockKey, string lockValue, TimeSpan timeSpan)
    {
        throw new NotImplementedException();
    }

    public bool IsAcquired()
    {
        throw new NotImplementedException();
    }

    public void Release()
    {
        throw new NotImplementedException();
    }
}