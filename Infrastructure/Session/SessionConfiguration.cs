using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Session;

public static class SessionConfiguration
{
    public static IServiceCollection ConfigureSession(this IServiceCollection services)
    {
        services.AddScoped<ISessionContext, SessionContext>();
        return services;
    }

    public static IApplicationBuilder ConfigureSessionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<SessionContextMiddleware>();
        return app;
    }
}
