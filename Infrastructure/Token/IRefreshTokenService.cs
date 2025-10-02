using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Token;

public interface IRefreshTokenService
{
    Task<(string token, DateTime expiresAt)> CreateRefreshTokenAsync(string userId, CancellationToken ct = default);
    Task<(bool valide, string? token, DateTime? expiresAt)> ValidateAndConsumeRefreshTokenAsync(string userId, string token, CancellationToken ct = default);
    Task RevokeRefreshTokenAsync(string userId, string token, CancellationToken ct = default);
}

