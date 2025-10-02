using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache;

public class MemoryCacheProvider : ICacheProvider
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheProvider(IMemoryCache memoryCache) => _memoryCache = memoryCache;

    public Task<T?> GetAsync<T>(string key, out bool haveValue)
    {
        if (_memoryCache.TryGetValue(key, out T? value))
        {
            haveValue = true; 
            return Task.FromResult(value);
        }

        haveValue = false;
        return Task.FromResult<T?>(default);
    }

    public Task SetAsync<T>(string key, T value, TimeSpan? ttl = null)
    {
        var options = new MemoryCacheEntryOptions();
        if (ttl.HasValue) 
            options.AbsoluteExpirationRelativeToNow = ttl.Value;
        _memoryCache.Set(key, value, options);
        return Task.CompletedTask;
    }

    public Task RemoveAsync(string key)
    {
        _memoryCache.Remove(key);
        return Task.CompletedTask;
    }
}
