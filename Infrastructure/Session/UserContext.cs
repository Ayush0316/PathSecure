using Infrastructure.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Session;

public class UserContext
{
    public string? UserId { get; init; }
    public string? Username { get; init; }
    public string? Email { get; init; }
    public List<string>? RoleIds { get; init; }
    public List<string>? Roles { get; init; }
    public List<string>? PermissionIds { get; init; }
    public List<string>? Permissions { get; init; }

    public UserContext(ClaimsPrincipal principal)
    {
        UserId = principal.FindFirstValue(TokenConsts.JWT_TOKEN_USER_ID);
        Username = principal.FindFirstValue(TokenConsts.JWT_TOKEN_USER_NAME);
        Email = principal.FindFirstValue(TokenConsts.JWT_TOKEN_USER_EMAIL);
        RoleIds = principal.FindAll(TokenConsts.JWT_TOKEN_ROLE_ID).Select(c => c.Value).ToList();
        Roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
        PermissionIds = principal.FindAll(TokenConsts.JWT_TOKEN_PERMISSION_ID).Select(c => c.Value).ToList();
        Permissions = principal.FindAll(TokenConsts.JWT_TOKEN_PERMISSION).Select(c => c.Value).ToList();
    }
}
