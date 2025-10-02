using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Token;

public static class TokenConsts
{
    public const string CACHE_ACTIVE_SIGNING_CREDENTIALS = "ActiveSigningCredentials";

    public const string JWT_ISSUER = "MyAppIssuer";

    public const string JWT_AUDIENCE = "MyAppAudience";

    public const string JWT_TOKEN_USER_ID = "UserId";

    public const string JWT_TOKEN_USER_NAME = "Username";

    public const string JWT_TOKEN_USER_EMAIL = "UserEmail";

    public const string JWT_TOKEN_ROLE_ID = "UserRoleId";

    public const string JWT_TOKEN_PERMISSION_ID = "UserPermissionId";

    public const string JWT_TOKEN_PERMISSION = "UserPermission";

    public static string CACHE_PUBLIC_KEY(string kid) => $"PublicKey:{kid}";
}
