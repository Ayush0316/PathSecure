using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Auth;

public class AuthConsts
{
    public const string BEARER_PREFIX = "Bearer ";
    public const string AUTH_SCHEME = "JWTSCHEME";
    public const string JWT_COOKIE_HEADER = "ACCESS_TOKEN";
    public const string REFRESH_COOKIE_HEADER = "REFRESH_TOKEN";

    public const string NOT_Authenticated = "ANONYMOUS";

    public static readonly string[] UnsecuredPaths = [
        "/api/auth/login",
        "/api/auth/silentLogin",
        "/api/auth/refresh",
        "/api/auth/client/token",
        "/api/auth/setPassword",
        "/swagger/v1/swagger.json",
        "/swagger/index.html"
    ];
}
