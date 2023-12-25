using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text;

namespace DevOpsFinalProject.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache) { _cache = cache; }

    public async Task<T?> GetAsync<T>(string key)
    {
        var body = await _cache.GetAsync(key);
        string? jsonString = null;
        if (body is not null)
            jsonString = Encoding.UTF8.GetString(body);
        if (jsonString is not null)
            return JsonSerializer.Deserialize<T?>(jsonString);
        T? result = default(T?);
        return result;
    }

    public async Task SetAsync<T>(string key, T obj, TimeSpan expiration)
    {
        var options = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = expiration };
        var json = JsonSerializer.Serialize(obj);
        var body = Encoding.UTF8.GetBytes(json);
        await _cache.SetAsync(key, body, options);
    }

    public async Task DeleteAsync(string key) { await _cache.RemoveAsync(key); }

    public async Task<bool> IsKeyExist(string key) => await _cache.GetStringAsync(key) != null;
}
