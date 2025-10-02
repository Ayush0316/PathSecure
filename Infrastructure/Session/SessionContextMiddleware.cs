using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Session;

public class SessionContextMiddleware
{
    private readonly RequestDelegate _next;
    public SessionContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext context, ISessionContext sessionContext)
    {
        sessionContext.Initialize(context);

        await _next(context);
    }
}
