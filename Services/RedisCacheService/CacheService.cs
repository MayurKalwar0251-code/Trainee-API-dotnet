using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (System.Exception)
        {
            Console.WriteLine("Redis isnt available, falling back to DB");
            return default;
        }
    }
    public async Task RemoveAsync(string key)
    {
        try
        {
            await _cache.RemoveAsync(key);
        }
        catch (System.Exception)
        {
            Console.WriteLine("Redis isnt available");
        }
    }
    public async Task SetAsync<T>(string key, T value, DistributedCacheEntryOptions? options = null)
    {
        try
        {
        await _cache.SetStringAsync(
            key,JsonSerializer.Serialize(value),
            options!
        );
        }
        catch (System.Exception)
        {
            Console.WriteLine("Redis isnt available, falling back to DB");
        }
    }
}