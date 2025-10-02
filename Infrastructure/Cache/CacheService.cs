using Infrastructure.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache;

public class CacheService : ICacheService
{
    private readonly ICacheProvider _provider;
    private readonly ISessionContext _session;

    public CacheService(ICacheProvider provider, ISessionContext session)
    {
        _provider = provider;
        _session = session;
    }

    public async Task<T?> FetchAsync<T>(string key, Func<Task<T>> fetch, TimeSpan? ttl = null, bool userSpecific = true)
    {
        if (userSpecific)
        {
            if(!_session.IsAuthenticated || _session.UserContext == null)
                throw new UnauthorizedAccessException("User must be authenticated to access user-specific cache.");
            
            key = $"User:{_session.UserContext.UserId}:{key}";
        }

        var value = await _provider.GetAsync<T?>(key, out bool haveValue);
        if (haveValue) return value;

        value = await fetch();
        if (value != null)
            await _provider.SetAsync(key, value, ttl);

        return value;
    }

    public Task RemoveAsync(string key) => _provider.RemoveAsync(key);
}
