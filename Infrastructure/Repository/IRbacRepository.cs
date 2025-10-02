using Infrastructure.Database;
using Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository;

public interface IRbacRepository
{
    Task<bool> HasPermissionOnResourcePathAsync(IList<String> permissionIds, string resourcePath, RbacResourceType resourceType, RbacAction action);
    Task<bool> HasPermissionOnPage(IList<string> permissionIds, string resourceName);
}