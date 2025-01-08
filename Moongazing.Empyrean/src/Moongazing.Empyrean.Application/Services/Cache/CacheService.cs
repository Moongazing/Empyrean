using StackExchange.Redis;
using System.Text.Json;

namespace Doing.Retail.Application.Services.Cache;

public class CacheService : ICacheService
{
    private readonly IConnectionMultiplexer redisConnection;
    private readonly IDatabase redisDatabase;

    public CacheService(IConnectionMultiplexer redisConnection)
    {
        this.redisConnection = redisConnection;
        redisDatabase = redisConnection.GetDatabase();
    }

    public async Task ClearCacheWithKeyAsync(string cacheGroupKey)
    {
        if (string.IsNullOrEmpty(cacheGroupKey))
        {
            throw new ArgumentException("Cache group key cannot be null or empty.");
        }

        RedisValue cachedGroup = await redisDatabase.StringGetAsync(cacheGroupKey);

        if (!cachedGroup.IsNullOrEmpty)
        {
            HashSet<string> keysInGroup = JsonSerializer.Deserialize<HashSet<string>>(cachedGroup!)!;

            foreach (string key in keysInGroup)
            {
                await redisDatabase.KeyDeleteAsync(key);
            }

            await redisDatabase.KeyDeleteAsync(cacheGroupKey);
            await redisDatabase.KeyDeleteAsync($"{cacheGroupKey}SlidingExpiration");
        }
    }

    public async Task ClearAllCacheAsync()
    {
        var server = redisConnection.GetServer(redisConnection.GetEndPoints().First());
        var keys = server.Keys(pattern: "*").Select(key => (string)key!).ToList();

        foreach (var key in keys)
        {
            await redisDatabase.KeyDeleteAsync(key);
        }
    }
}