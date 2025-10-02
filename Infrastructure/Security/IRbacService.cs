using Infrastructure.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public interface IRbacService
{
    Task<bool> IsAllowedAsync(RbacResource resource, RbacAction action);
    Task<bool> IsAllowedAsync(UserContext userContext, RbacResource resource, RbacAction action);
}
