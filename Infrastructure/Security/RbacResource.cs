using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public class RbacResource
{
    public string ResourceName { get; set; }
    public RbacResourceType Type { get; set; }
    public RbacResource()
    {
    }
    public RbacResource(string resourceName, RbacResourceType type)
    {
        ResourceName = resourceName;
        Type = type;
    }

    public static RbacResource API(string path)
    {
        return new RbacResource(path, RbacResourceType.API);
    }

    public static RbacResource PAGE(string pageName)
    {
        return new RbacResource(pageName, RbacResourceType.PAGE);
    }
    public override string ToString()
    {
        return $"{Type}:{ResourceName}";
    }
    public override bool Equals(object? obj)
    {
        if (obj is not RbacResource other)
            return false;

        return ResourceName.Equals(other.ResourceName, StringComparison.OrdinalIgnoreCase) &&
               Type == other.Type;
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(ResourceName.ToLowerInvariant(), Type);
    }

    public static implicit operator RbacResource(string resourceString)
    {
        var parts = resourceString.Split(':');
        if (parts.Length != 2)
        {
            throw new ArgumentException("Invalid resource string format. Expected format: 'ResourceName:Action'");
        }
        return new RbacResource(parts[1], RbacResourceTypeExtensions.Parse(parts[0]));
    }

    public static implicit operator string(RbacResource resource)
    {
        return resource.ToString();
    }

}
