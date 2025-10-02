using Domain.Entities;
using Infrastructure.Crypto;
using Infrastructure.Exceptions;
using Infrastructure.Repository;
using Infrastructure.Session;
using Infrastructure.Token;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Org.BouncyCastle.Crypto.Operators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly IPasswordSetLinkRepository _passwordSetLinkRepo;
    private readonly IRefreshTokenRepository _refreshRepo;
    private readonly IOAuthClientRepository _oauthClientRepo;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshService;
    private readonly ISessionContext _session;
    public AuthService(IUserRepository userRepo,IPasswordSetLinkRepository pslr, IRefreshTokenRepository refresh, IOAuthClientRepository oauthClientRepo , IJwtService jwtService, IRefreshTokenService refreshService, ISessionContext session)
    {
        _userRepo = userRepo;
        _passwordSetLinkRepo = pslr;
        _refreshRepo = refresh;
        _oauthClientRepo = oauthClientRepo;
        _jwtService = jwtService;
        _refreshService = refreshService;
        _session = session;
    }
    public async Task<(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)> AuthenticateAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(username));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(password));

        var user = await VerifyUsernameAndPassword(username, password);

        return await GenerateTokens(user);
    }

    public async Task<(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)> AuthenticateAsync(string? username, string? password, string? userId, string? token)
    {
        User user;
        if (!String.IsNullOrEmpty(userId) && !String.IsNullOrEmpty(token))
        {
            user = await VerifyUserIdAndToken(userId, token);
        }
        else if (!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
        {
            user = await VerifyUsernameAndPassword(username, password, UserType.SILENT);
        }
        else
            throw new InvalidOperationException("Required parameters are missing");

         return await GenerateTokens(user);
    }

    public async Task<(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)> RefreshTokenAsync(string userId, string refreshToken)
    {

        (bool valide, string? newToken, DateTime? expAt) = await _refreshService.ValidateAndConsumeRefreshTokenAsync(userId, refreshToken);

        if (!valide)
            throw new InvalidOperationException("Invalid token provided");

        (var accessToken, var accessExpiresAt) = await _jwtService.CreateJwtTokenAsync(userId);

        return (accessToken, accessExpiresAt, newToken!, expAt!.Value);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        string? userId = _session.UserContext?.UserId;
        if (userId == null)
            throw new InvalidOperationException("User not authenticated");

        await _refreshService.RevokeRefreshTokenAsync(userId!, refreshToken, ct);
    }

    public async Task SetPassword(string userId, string rawToken, string password)
    {
        var userEntity = await _userRepo.FindByPrimaryKey(userId);
        if (userEntity == null)
            throw new InvalidOperationException("No such user exists");

        if (userEntity.Status != UserStatus.NOT_VERIFIED)
            throw new InvalidOperationException("Password already set");

        rawToken.Trim();
        var hashRawToken = CryptoHelpers.HashToken(rawToken);

        var tokenEntity = await _passwordSetLinkRepo.Query.Where(l => l.TokenHash == hashRawToken && l.Expiry >= DateTime.Now).FirstOrDefaultAsync();
        if (tokenEntity == null)
            throw new InvalidOperationException("Invalid password set link");

        tokenEntity.Used = true;

        if (!BCryptService.IsValidPassword(password))
            throw new InvalidOperationException("Password is not valid");

        userEntity.PasswordHash = BCryptService.HashPassword(password);
        userEntity.Status = UserStatus.VERIFIED;
        await _userRepo.SaveChanges();
    }

    public async Task<(string token, DateTime exp)> ClientTokenRequest(string clientId, string clientSecret, string userId)
    {
        if (String.IsNullOrEmpty(clientId))
            throw new InvalidOperationException("Client id is missing");

        if (String.IsNullOrEmpty(clientSecret))
            throw new InvalidOperationException("Client secret is missing");

        var client = _oauthClientRepo.Query.Where(c => c.ClientId == clientId).FirstOrDefault();

        if (client == null)
            throw new InvalidOperationException("Client Id is not correct");

        var verified = BCryptService.VerifyPassword(clientSecret, client.ClientSecretHash);

        if (!verified)
            throw new UnauthorizedAccessException("Client Id or Client secret is incorrect");

        var user = await _userRepo.FindByPrimaryKey(userId);
        if (user == null)
            throw new InvalidOperationException("User id is invalid");

        return await _jwtService.CreateJwtTokenAsync(userId);
    }

    private async Task<User> VerifyUsernameAndPassword(string username, string password, UserType allowedType = UserType.REGULAR) {
        var userEntity = await _userRepo.FindByUsernameAsync(username);

        if (userEntity == null)
            throw new NotFoundException($"User with username: {username} not found");

        IsLoginAllowed(userEntity, allowedType);

        bool verified = BCryptService.VerifyPassword(password, userEntity.PasswordHash!);

        if (!verified)
            throw new UnauthorizedAccessException("Invalid username or password");

        return userEntity;
    }

    private async Task<User> VerifyUserIdAndToken(string userId, string token)
    {
        var userEntity = await _userRepo.FindByPrimaryKey(userId);
        if (userEntity == null)
            throw new NotFoundException($"User with username: {userId} not found");

        IsLoginAllowed(userEntity, UserType.OAUTH2);

        var claims = _jwtService.ValidateJwtTokenAsync(token);

        string? claimsUserId = claims?.FindFirst("UserId")?.ToString();

        if (claimsUserId == null || !String.Equals(claimsUserId, userId))
            throw new UnauthorizedAccessException("Invalid token provided");

        return userEntity;
    }

    private void IsLoginAllowed(User user, UserType allowedType)
    {
        if (user.Status == UserStatus.NOT_VERIFIED)
            throw new UnauthorizedAccessException("User is not verified");

        if (user.Status == UserStatus.IN_ACTIVE)
            throw new UnauthorizedAccessException("User is disabled");

        if (user.Type != allowedType)
            throw new UnauthorizedAccessException("Invalid login method");
    }

    private async Task<(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)> GenerateTokens(User user)
    {
        (var accessToken, var accessExpiresAt) = await _jwtService.CreateJwtTokenAsync(user.Id);
        (var refreshToken, var refreshExpiresAt) = await _refreshService.CreateRefreshTokenAsync(user.Id);

        return (accessToken, accessExpiresAt, refreshToken, refreshExpiresAt);
    }
}
