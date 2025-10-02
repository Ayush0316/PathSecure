using Infrastructure.Database;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Session;

public interface ISessionContext
{
    Guid SessionId { get; }
    string? Source { get; }
    bool IsAuthenticated { get; }
    UserContext? UserContext { get; }
    void Initialize(HttpContext context);
}
