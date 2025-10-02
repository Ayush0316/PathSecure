using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache;

public interface ICacheService
{
    Task<T?> FetchAsync<T>(string key, Func<Task<T>> fetch, TimeSpan? ttl = null, bool userSpecific = true);
    Task RemoveAsync(string key);
}