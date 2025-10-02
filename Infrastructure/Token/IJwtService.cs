using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Token;
public interface IJwtService
{
    Task<(string token, DateTime expiresAt)> CreateJwtTokenAsync(string userId);
    Task<(string token, DateTime expiresAt)> CreateJwtTokenAsync(string userId, TimeSpan ttl);
    Task<(string token, DateTime expiresAt)> CreateJwtTokenAsync(string userId, IEnumerable<Claim> claims, TimeSpan ttl);
    ClaimsPrincipal? ValidateJwtTokenAsync(string token, CancellationToken ct = default);
}
