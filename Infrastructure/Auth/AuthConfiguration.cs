using Infrastructure.Security;
using Infrastructure.Session;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth;

public static class AuthConfiguration
{
    
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection service)
    {
        service.ConfigureToken();
        service.ConfigureSession();

        service.AddScoped<IRbacService, RbacService>();

        service.AddAuthentication(AuthConsts.AUTH_SCHEME).
            AddScheme<AuthenticationSchemeOptions, JwtAuthHandler>(AuthConsts.AUTH_SCHEME, options => {
                
            });

        service.AddAuthorization(options =>
        {
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .Build();
        });

        return service;
    }

    public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder app)
    {
        app.UseWhen(context => !AuthConsts.UnsecuredPaths.Contains(context.Request.Path.Value), appBranch =>
        {
            appBranch.UseAuthentication();
            appBranch.UseAuthorization();
            appBranch.ConfigureSessionMiddleware();
            appBranch.UseMiddleware<RbacAuthorizationMiddleware>();
        }); 
        return app;
    }
}