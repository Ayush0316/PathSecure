using Infrastructure.Auth;
using Infrastructure.Cache;
using Infrastructure.Database;
using Infrastructure.Mailer;
using Infrastructure.Repository;
using Infrastructure.Session;
using Infrastructure.Token;
using MailKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Update.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure;

public static class InfrastructureConfiguration
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfigurationManager config)
    {
        services.AddDbContext<BaseDbContext>(options =>
        {
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IEmailSender, SmtpEmailSender>();

        // Add DI for Repositories here
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISigninKeyRepository, SigninKeyRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<IRbacRepository, RbacRepository>();
        services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        services.AddScoped<IPasswordSetLinkRepository, PasswordSetLinkRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IOAuthClientRepository, OAuthClientRepository>();

        services.ConfigureCache();
        services.ConfigureAuthentication();

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseAuthenticationMiddleware();
        return app;
    }
}
