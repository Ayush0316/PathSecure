using Domain.Entities;
using Infrastructure.Auth;
using Infrastructure.Cache;
using Infrastructure.Database;
using Infrastructure.Repository;
using Infrastructure.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Infrastructure.Security;

public class RbacService : IRbacService
{
    private readonly ISessionContext _sessionContext;
    private readonly IRbacRepository _rbacRepository;
    private readonly ICacheService _cache;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

    public RbacService(ISessionContext sessionContext, IRbacRepository rbacRepository, ICacheService cacheService)
    {
        _sessionContext = sessionContext;
        _rbacRepository = rbacRepository;
        _cache = cacheService;
    }
    public Task<bool> IsAllowedAsync(RbacResource resource, RbacAction action)
    {
        var user = _sessionContext.UserContext;

        if (user == null ||  String.IsNullOrEmpty(user.UserId))
            throw new InvalidOperationException("No valid user context available");

        return IsAllowedAsync(user!, resource, action);
    }

    public async Task<bool> IsAllowedAsync(UserContext userContext, RbacResource resource, RbacAction action)
    {
        if(userContext == null || String.IsNullOrEmpty(userContext.UserId))
            throw new InvalidOperationException("No valid user context available");

        if(userContext.RoleIds == null || userContext.RoleIds.Count == 0)
            throw new UnauthorizedAccessException("User has no roles assigned");

        if(userContext.PermissionIds == null || userContext.PermissionIds.Count == 0)
            throw new UnauthorizedAccessException("User has no permissions assigned");

        if(userContext?.Email?.EndsWith(AuthConsts.ADMIN_DOMAIN, StringComparison.OrdinalIgnoreCase) == true)
            return true;

        if (resource.Type == RbacResourceType.PAGE)
        {
            return await CheckForPage(userContext!, resource);
        }else
        {
            return await CheckForApi(userContext!, resource, action);
        }
    }

    private async Task<bool> CheckForApi(UserContext userContext, RbacResource resource, RbacAction action)
    {
        var cacheKey = $"{RbacConsts.PERMISSION_CACHE_KEY_PREFIX}{resource.Type}:{resource.ResourceName}:{action}";

        return await _cache.FetchAsync(
            cacheKey,
            async () =>
            {
                var hasPermission = await _rbacRepository.HasPermissionOnResourcePathAsync(
                    userContext.PermissionIds!,
                    resource.ResourceName,
                    resource.Type,
                    action);

                return hasPermission;
            },
        _cacheDuration
        );
    }

    private async Task<bool> CheckForPage(UserContext userContext, RbacResource resource)
    {
        var cacheKey = $"{RbacConsts.PERMISSION_CACHE_KEY_PREFIX}{resource.Type}:{resource.ResourceName}";

        return await _cache.FetchAsync(
            cacheKey,
            async () =>
            {
                var hasPermission = await _rbacRepository.HasPermissionOnPage(
                    userContext.PermissionIds!,
                    resource.ResourceName);

                return hasPermission;
            },
        _cacheDuration
        );
    }
}
