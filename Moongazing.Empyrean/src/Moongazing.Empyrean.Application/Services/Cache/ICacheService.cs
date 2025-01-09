namespace Moongazing.Empyrean.Application.Services.Cache;

public interface ICacheService
{
    Task ClearCacheWithKeyAsync(string cacheGroupKey);
    Task ClearAllCacheAsync();
}
