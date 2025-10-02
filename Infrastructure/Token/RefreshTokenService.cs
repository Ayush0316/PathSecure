using Domain.Entities;
using Infrastructure.Crypto;
using Infrastructure.Database;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Token;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly IRefreshTokenRepository _repo;
    private readonly TimeSpan _refreshLifetime = TimeSpan.FromDays(30);

    public RefreshTokenService(IRefreshTokenRepository repo) => _repo = repo;

    public async Task<(string token, DateTime expiresAt)> CreateRefreshTokenAsync(string userId, CancellationToken ct = default)
    {
        var result = CreateNewToken(userId);
        var entity = result.entity;
        await _repo.Create(entity);
        return (result.token, entity.ExpiresAt);
    }

    public async Task<(bool valide, string? token, DateTime? expiresAt)> ValidateAndConsumeRefreshTokenAsync(string userId, string token, CancellationToken ct = default)
    {
        var hash = CryptoHelpers.HashToken(token);

        var entity = await _repo.Query
            .Where(r => r.UserId == userId &&
                    r.TokenHash == hash &&
                    !r.Revoked &&
                    !r.Used &&
                    r.ExpiresAt > DateTime.UtcNow)
            .FirstOrDefaultAsync(ct);

        if (entity == null) return (false, null, null);

        var result = CreateNewToken(userId);
        var newEntity = result.entity;

        entity.Used = true;
        entity.ReplacedByHash = newEntity.TokenHash;

        await _repo.Create(newEntity, false);
        await _repo.SaveChanges();
        return (true, result.token, newEntity.ExpiresAt);
    }

    public async Task RevokeRefreshTokenAsync(string userId, string token, CancellationToken ct = default)
    {
        var hash = CryptoHelpers.HashToken(token);
        var entity = await _repo.Query
            .Where(r => r.UserId == userId && r.TokenHash == hash)
            .FirstOrDefaultAsync(ct);
        if (entity == null) return;
        entity.Revoked = true;
        await _repo.SaveChanges();
    }

    private (RefreshToken entity, string token) CreateNewToken(string userId)
    {
        var token = CryptoHelpers.GenerateRandomToken(64);
        var hashed = CryptoHelpers.HashToken(token);
        var entity = new RefreshToken
        {
            UserId = userId,
            TokenHash = hashed,
            ExpiresAt = DateTime.UtcNow.Add(_refreshLifetime),
        };
        return (entity, token);
    }
}
