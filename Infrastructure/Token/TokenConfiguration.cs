using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Token;

public static class TokenConfiguration
{
    public static IServiceCollection ConfigureToken(this IServiceCollection service)
    {
        service.AddScoped<IKeyManagementService, KeyManagementService>();
        service.AddScoped<IRefreshTokenService, RefreshTokenService>();
        service.AddScoped<IJwtService, JwtService>();
        return service;
    }
}
