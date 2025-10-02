using Domain.Entities;
using Infrastructure.Database;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public class RbacRepository : IRbacRepository
{
    private readonly BaseDbContext _dc;

    public RbacRepository(BaseDbContext dc)
    {
        _dc = dc;
    }

    public async Task<bool> HasPermissionOnResourcePathAsync(IList<String> permissionIds, string resourcePath, RbacResourceType resourceType, RbacAction action)
    {
        var permissionIdsList = permissionIds.ToList();

        if(permissionIdsList.Count == 0)
            return false;

        // Build hierarchy paths: for "api/users/123" -> ["api/users/123", "api/users", "api", "*"]
        var hierarchyPaths = BuildResourceHierarchyPaths(resourcePath);

        var hasPermission = await _dc.Resources
            .Where(r =>
                r.ResourceType == resourceType.ToFriendlyString() &&
                hierarchyPaths.Contains(r.ResourceName) &&
                (r.ResourceMethod == action.ToString() || 
                    r.ResourceMethod == RbacAction.ALL.ToString()) &&
                (r.ResourcePermissions.Any(s =>
                    permissionIdsList.Contains(s.PermissionId))
                    || (r.ParentResource != null &&
                        r.ParentResource.ResourcePermissions.Any(s =>
                        permissionIdsList.Contains(s.PermissionId))
                        )
                )
            )
            .AnyAsync();

        return hasPermission;
    }

    public async Task<bool> HasPermissionOnPage(IList<string> permissionIds, string resourceName)
    {
        var permissionIdsList = permissionIds.ToList();

        if (permissionIdsList.Count == 0)
            return false;

        var hasPermission = await _dc.Resources
            .Where(r =>
                r.ResourceType == RbacResourceType.PAGE.ToFriendlyString() &&
                r.ResourceName == resourceName &&
                (r.ResourcePermissions.Any(s =>
                    permissionIdsList.Contains(s.PermissionId))
                    || (r.ParentResource != null &&
                        r.ParentResource.ResourcePermissions.Any(s =>
                        permissionIdsList.Contains(s.PermissionId))
                        )
                )
            )
            .AnyAsync();

        return hasPermission;
    }

    private static List<string> BuildResourceHierarchyPaths(string resourcePath)
    {
        var paths = new List<string> { "*" }; // Always include wildcard

        // Add the exact path
        paths.Add(resourcePath);

        // Build parent paths by removing segments from the end
        // For "/api/users/123" -> "/api/users", "/api"
        var hasLeadingSlash = resourcePath.StartsWith('/');
        var segments = resourcePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        for (int i = segments.Length - 1; i > 0; i--)
        {
            var parentPath = string.Join("/", segments.Take(i));
            if (hasLeadingSlash)
            {
                parentPath = "/" + parentPath;
            }
            paths.Add(parentPath);
        }

        return paths;
    }
}
