using Infrastructure.Auth;
using Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Session;

public class SessionContext : ISessionContext
{
    public Guid SessionId { get; private set; }

    public string? Source { get; private set; }

    public UserContext? UserContext { get; private set; }
    public bool IsAuthenticated { get; private set; } = false;

    public bool _isInitialized = false;
    public void Initialize(HttpContext context)
    {
        if(_isInitialized) 
            throw new InvalidOperationException("SessionContext has already been initialized and is immutable.");

        _isInitialized = true;
        SessionId = Guid.NewGuid();
        var principal = context?.User;
        Source = context?.Request.Path.Value;
        IsAuthenticated = CheckIsAuthenticated(principal);
        UserContext = principal != null ? new UserContext(principal!) : null;
    }

    private bool CheckIsAuthenticated(ClaimsPrincipal? principal)
    {
        if (principal == null) return false;

        bool haveRole = principal!.IsInRole(AuthConsts.NOT_Authenticated);
        return !haveRole;
    }
}
