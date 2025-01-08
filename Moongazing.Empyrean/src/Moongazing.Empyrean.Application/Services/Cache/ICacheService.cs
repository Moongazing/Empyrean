namespace Doing.Retail.Application.Services.Cache;

public interface ICacheService
{
    Task ClearCacheWithKeyAsync(string cacheGroupKey);
    Task ClearAllCacheAsync();
}
