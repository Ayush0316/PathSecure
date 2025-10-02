using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public interface IAuthService
{
    Task<(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)> AuthenticateAsync(string username, string password);
    Task<(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)> AuthenticateAsync(string? username, string? password, string? userId, string? token);
    Task<(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)> RefreshTokenAsync(string userId, string refreshToken);
    Task<(string token, DateTime exp)> ClientTokenRequest(string clientId, string clientSecret, string userId);
    Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    Task SetPassword(string userId, string rawToken, string password);
}
