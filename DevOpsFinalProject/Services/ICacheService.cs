namespace DevOpsFinalProject.Services;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string key);

    Task SetAsync<T>(string key, T obj, TimeSpan expiration);

    Task DeleteAsync(string key);

    Task<bool> IsKeyExist(string key);
}
