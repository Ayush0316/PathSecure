using Infrastructure.Token;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Infrastructure.Auth;

public class JwtAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly ILogger<JwtAuthHandler> _logger;
    private readonly IJwtService _jwtService;
    public JwtAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, IJwtService jwtService)
        : base(options, logger, encoder)
    {
        _logger = logger.CreateLogger<JwtAuthHandler>();
        _jwtService = jwtService;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (AuthConsts.UnsecuredPaths.Contains(Request.Path.Value))
        {
            var identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Role, AuthConsts.NOT_Authenticated)
            }, Scheme.Name);

            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        _logger.LogTrace("JwtAuthHandler: Starting authentication for path: {Path}", Request.Path);

        var token = Request.Cookies[AuthConsts.JWT_COOKIE_HEADER];
        _logger.LogTrace("JwtAuthHandler: Cookie token present: {HasToken}", !string.IsNullOrEmpty(token));

        if (string.IsNullOrEmpty(token))
        {
            token = Request.Headers.Authorization.ToString();
            if (!string.IsNullOrEmpty(token) && token.StartsWith(AuthConsts.BEARER_PREFIX))
            {
                token = token[AuthConsts.BEARER_PREFIX.Length..];
            }
        }

        if (string.IsNullOrEmpty(token))
        {

            _logger.LogTrace("JwtAuthHandler: Invalid token received");
            return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));
        }

        try
        {
            var principal = _jwtService.ValidateJwtTokenAsync(token);
            if (principal == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Token"));
            }
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "JwtAuthHandler: Token validation failed with error: {Error}", ex.Message);
            return Task.FromResult(AuthenticateResult.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "JwtAuthHandler: Token validation failed with unexpected error: {Error}", ex.Message);
            return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
        }
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        Response.ContentType = "application/json";
        var result = new ResponseAuth("Invalid/No token");
        await Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
    }
}
