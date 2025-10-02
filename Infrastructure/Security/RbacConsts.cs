using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public class RbacConsts
{
    public static readonly string PERMISSION_CACHE_KEY_PREFIX = "RBAC_PERMISSION_"; }

public enum RbacResourceType
{
    API,
    TABLE,
    PAGE
}

public static class RbacResourceTypeExtensions
{
    public static RbacResourceType Parse(string value)
    {
        if (!Enum.TryParse<RbacResourceType>(value, true, out var result))
        {
            throw new ArgumentException($"Invalid resource type: {value}");
        }
        return result;
    }

    public static string ToFriendlyString(this RbacResourceType resourceType)
    {
        return resourceType.ToString().ToLowerInvariant();
    }
}

public enum RbacAction
{
    ALL,
    CREATE,
    READ,
    UPDATE,
    DELETE,
    PAGE
}

public static class RbacActionExtensions
{
    public static RbacAction Parse(string value)
    {
        if (!Enum.TryParse<RbacAction>(value, true, out var result))
        {
            throw new ArgumentException($"Invalid resource type: {value}");
        }
        return result;
    }
}