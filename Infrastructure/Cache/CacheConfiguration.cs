using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cache;

public static class CacheConfiguration
{
    public static IServiceCollection ConfigureCache(this IServiceCollection service)
    {
        service.AddMemoryCache();
        service.AddScoped<ICacheService, CacheService>();
        service.AddScoped<ICacheProvider, MemoryCacheProvider>();
        return service;
    }
}
