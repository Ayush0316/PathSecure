using Infrastructure.Auth;
using Infrastructure.Session;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Security;

public class RbacAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    public RbacAuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, ISessionContext session, IRbacService rbacService)
    {
        if (ShouldSkipRbacCheck(context))
        {
            await _next(context);
        }

        if (context?.User?.Identity?.IsAuthenticated == false)
        {
            await WriteResponse(context, 401, "User is not authenticated");
            return;
        }

        var userContext = session.UserContext;

        if (userContext == null || String.IsNullOrEmpty(userContext.UserId))
        {
            await WriteResponse(context!, 401, "No valid user context available");
            return;
        }

        var (resource, action) = DetermineResourceAndAction(context!);

        if(String.IsNullOrEmpty(resource))
        {
            await WriteResponse(context!, 400, "Could not determine resource from request");
            return;
        }

        var isAllowed = await rbacService.IsAllowedAsync(RbacResource.API(resource), action);

        if(!isAllowed)
        {
            await WriteResponse(context!, 403, "Insufficient permissions");
            return;
        }

        context!.Items["RbacResource"] = resource;
        context!.Items["RbacAction"] = action;

        await _next(context);
    }
    private static bool ShouldSkipRbacCheck(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        if (AuthConsts.UnsecuredPaths.Contains(path))
        {
            return true;
        }

        return false;
    }

    private static async Task WriteResponse(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        var response = new
        {
            IsSuccess = false,
            message,
            StatusCode = statusCode
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static (string resource, RbacAction action) DetermineResourceAndAction(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;
        var method = context.Request.Method.ToUpperInvariant();

        // Apply resource mappings first
        var resource = DetermineResource(path);

        // Determine action based on HTTP method
        var action = DetermineAction(method);

        return (resource, action);
    }
    private static RbacAction DetermineAction(string httpMethod)
    {
        return httpMethod switch
        {
            "GET" => RbacAction.READ,
            "POST" => RbacAction.CREATE,
            "PUT" => RbacAction.UPDATE,
            "PATCH" => RbacAction.UPDATE,
            "DELETE" => RbacAction.DELETE,
            "HEAD" => RbacAction.READ,
            "OPTIONS" => RbacAction.READ,
            _ => RbacAction.READ
        };
    }

    private static string DetermineResource(string path)
    {
        // Normalize path - remove trailing slashes and convert to lowercase
        return path.TrimEnd('/').ToLowerInvariant();
    }
}