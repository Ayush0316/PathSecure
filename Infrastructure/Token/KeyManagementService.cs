using Domain.Entities;
using Infrastructure.Cache;
using Infrastructure.Database;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Token;

public class KeyManagementService : IKeyManagementService
{
    public readonly ISigninKeyRepository _repo;
    public readonly ICacheService _cache;
    public readonly TimeSpan _cacheDuration = TimeSpan.FromDays(1);
    public KeyManagementService(ISigninKeyRepository repo, ICacheService cache)
    {
        _repo = repo;
        _cache = cache;
    }
    public async Task<string> CreateAndActivateNewRsaKeyAsync()
    {
        using var rsa = RSA.Create(2048);
        var kid = Guid.NewGuid().ToString("N");
        
        var activeKeys = await _repo.Query.Where(k => k.IsActive).ToListAsync();
        foreach (var key in activeKeys)
        {
            key.IsActive = false;
            key.RetiredAt = DateTime.UtcNow;
        }

        var newKey = new SigningKey
        {
            Kid = kid,
            Algorithm = SecurityAlgorithms.RsaSha256,
            PublicKeyPem = rsa.ExportRSAPublicKeyPem(),
            PrivateKeyEncrypted = rsa.ExportRSAPrivateKeyPem(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.Create(newKey, false);
        await _repo.SaveChanges();
        await _cache.RemoveAsync(TokenConsts.CACHE_ACTIVE_SIGNING_CREDENTIALS);
        return kid;
    }

    public async Task<SigningCredentials> GetActiveSigningCredentialsAsync()
    {
        var signCred = await _cache.FetchAsync(
            TokenConsts.CACHE_ACTIVE_SIGNING_CREDENTIALS, 
            async () =>
                {
                    var activeKey = await _repo.Query.Where(k => k.IsActive).OrderByDescending(k => k.CreatedAt).FirstOrDefaultAsync();
                    if (activeKey == null)
                        throw new InvalidOperationException("No active signing key found.");
                    var rsa = RSA.Create();
                    rsa.ImportFromPem(activeKey.PrivateKeyEncrypted);
                    var key = new RsaSecurityKey(rsa)
                    {
                        KeyId = activeKey.Kid
                    };
                    return new SigningCredentials(key, activeKey.Algorithm);
                }, 
            _cacheDuration, 
            userSpecific: false
        );

        if (signCred == null)
            throw new InvalidOperationException("Failed to retrieve active signing credentials.");

        return signCred!;
    }

    public async Task<SecurityKey?> GetPublicKeyByKidAsync(string kid)
    {
        var result = await _cache.FetchAsync(
            TokenConsts.CACHE_PUBLIC_KEY(kid),
            async () =>
                {
                    var publicKeyStr = await _repo.Query.Where(k => k.Kid == kid)
                        .Select(k => k.PublicKeyPem)
                        .FirstOrDefaultAsync();

                    if (publicKeyStr == null)
                        return null;

                    var rsa = RSA.Create();
                    rsa.ImportFromPem(publicKeyStr);
                    var key = new RsaSecurityKey(rsa)
                    {
                        KeyId = kid
                    };
                    return key;
                },
            _cacheDuration,
            userSpecific: false
        );
        return result;
    }

    public Task RotateKeysAsync()
    {
        return CreateAndActivateNewRsaKeyAsync();
    }
}
