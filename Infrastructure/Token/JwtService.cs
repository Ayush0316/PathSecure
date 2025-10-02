using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Experimental;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Token;

public class JwtService : IJwtService
{
    private readonly IKeyManagementService _keyManagementService;
    private readonly IUserRepository _userRepository;

    public JwtService(IKeyManagementService keyManagementService, IUserRepository userRepository)
    {
        _keyManagementService = keyManagementService;
        _userRepository = userRepository;
    }

    public async Task<(string token, DateTime expiresAt)> CreateJwtTokenAsync(string userId, TimeSpan ttl)
    {
        List<Claim> claims = await BuildUserClaims(userId);
        return await CreateJwtTokenAsync(userId, claims, ttl);
    }
    public Task<(string token, DateTime expiresAt)> CreateJwtTokenAsync(string userId)
    {
        return CreateJwtTokenAsync(userId, TimeSpan.FromMinutes(15));
    }

    public async Task<(string token, DateTime expiresAt)> CreateJwtTokenAsync(string userId, IEnumerable<Claim> claims, TimeSpan ttl)
    {
        var cred = await _keyManagementService.GetActiveSigningCredentialsAsync();

        var expiresAt = DateTime.UtcNow.Add(ttl);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = TokenConsts.JWT_ISSUER,
            Audience = TokenConsts.JWT_AUDIENCE,
            Subject = new ClaimsIdentity(claims),
            NotBefore = DateTime.UtcNow,
            Expires = expiresAt,
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = cred
        };

        JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return (tokenHandler.WriteToken(token),expiresAt);
    }

    public ClaimsPrincipal? ValidateJwtTokenAsync(string token, CancellationToken ct = default)
    {
        var parameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            RequireSignedTokens = true,
            RequireExpirationTime = true,
            ValidateIssuer = true,
            ValidIssuer = TokenConsts.JWT_ISSUER,
            ValidateAudience = true,
            ValidAudience = TokenConsts.JWT_AUDIENCE,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromMinutes(1),
            IssuerSigningKeyResolver = (jwtToken, securityToken, kid, validationParameters) =>
            {
                var key = _keyManagementService.GetPublicKeyByKidAsync(kid).GetAwaiter().GetResult();
                return key != null ? new[] { key } : Array.Empty<SecurityKey>();
            }
        };

        var handler = new JwtSecurityTokenHandler();
        try
        {
            return handler.ValidateToken(token, parameters, out _);
        } catch
        {
            return null;
        }

        // TODO: Handle different exceptions and return more specific error messages

        //catch (SecurityTokenExpiredException)
        //{
        //    throw new UnauthorizedAccessException(JwtManagerConsts.ErrorTokenExpired);
        //}
        //catch (SecurityTokenInvalidSignatureException)
        //{
        //    throw new UnauthorizedAccessException(JwtManagerConsts.ErrorInvalidSignature);
        //}
        //catch (SecurityTokenInvalidIssuerException)
        //{
        //    throw new UnauthorizedAccessException(string.Format(JwtManagerConsts.ErrorInvalidIssuer, issuer));
        //}
        //catch (SecurityTokenInvalidAudienceException)
        //{
        //    throw new UnauthorizedAccessException(string.Format(JwtManagerConsts.ErrorInvalidAudience, audience));
        //}
        //catch (SecurityTokenNotYetValidException)
        //{
        //    throw new UnauthorizedAccessException(JwtManagerConsts.ErrorTokenNotYetValid);
        //}
        //catch (SecurityTokenNoExpirationException)
        //{
        //    throw new UnauthorizedAccessException(JwtManagerConsts.ErrorNoExpiration);
        //}
        //catch (SecurityTokenInvalidLifetimeException ex)
        //{
        //    throw new UnauthorizedAccessException(string.Format(JwtManagerConsts.ErrorInvalidLifetime, ex.Message));
        //}
        //catch (SecurityTokenMalformedException ex)
        //{
        //    throw new UnauthorizedAccessException(string.Format(JwtManagerConsts.ErrorMalformedToken, ex.Message));
        //}
        //catch (SecurityTokenException ex)
        //{
        //    throw new UnauthorizedAccessException(string.Format(JwtManagerConsts.ErrorTokenValidationFailed, ex.Message), ex);
        //}
        //catch (ArgumentException ex)
        //{
        //    throw new UnauthorizedAccessException(string.Format(JwtManagerConsts.ErrorInvalidArguments, ex.Message), ex);
        //}
        //catch (Exception ex) when (!(ex is UnauthorizedAccessException))
        //{
        //    throw new UnauthorizedAccessException(string.Format(JwtManagerConsts.ErrorUnexpected, ex.Message), ex);
        //}
    }

    private async Task<List<Claim>> BuildUserClaims(string userId)
    {
        List<Claim> claims = new List<Claim>();
        
        var userEntity = await _userRepository.FindByPrimaryKey(userId);
        if (userEntity == null)
            throw new InvalidOperationException($"No user found with user id: {userId}");

        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(TokenConsts.JWT_TOKEN_USER_ID, userEntity!.Id));
        claims.Add(new Claim(TokenConsts.JWT_TOKEN_USER_NAME, userEntity.Username));
        claims.Add(new Claim(TokenConsts.JWT_TOKEN_USER_EMAIL, userEntity.Email));

        if (userEntity.UserRoles?.Any() == true)
        {
            var userRoles = userEntity.UserRoles;
            claims.AddRange(userRoles.Select(ur => new Claim(ClaimTypes.Role, ur.Role.Name)));
            claims.AddRange(userRoles.Select(ur => new Claim(TokenConsts.JWT_TOKEN_ROLE_ID, ur.Role.Id)));
            
            var permissions = userRoles
                .SelectMany(ur => ur.Role.RolePermissions)
                .Select(rp => (rp.Permission.Name, rp.Permission.Id))
                .Distinct();

            claims.AddRange(permissions.Select(pr => new Claim(TokenConsts.JWT_TOKEN_PERMISSION_ID, pr.Id)));
            claims.AddRange(permissions.Select(pr => new Claim(TokenConsts.JWT_TOKEN_PERMISSION, pr.Name)));
        }

        return claims;
    }
}
