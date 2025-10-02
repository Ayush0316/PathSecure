using Infrastructure.Auth;
using Infrastructure.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;
using Service.DTO;
using Service.Services;

namespace Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authService;
    private readonly IKeyManagementService ks;
    private readonly string RefreshPath = "/api/auth/";

    public AuthController(IAuthService authService, IKeyManagementService ks)
    {
        _authService = authService;
        this.ks = ks;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto login)
    {
        var res = await _authService.AuthenticateAsync(login.Username, login.Password);
        SetTokenCookie(res.token, res.expiresAt, res.refreshToken, res.refreshTokenExpiresAt);
        return Ok(new ResponseDto());
    }

    [HttpPost("silentLogin")]
    public async Task<IActionResult> LoginSilent(SilentLoginDto dto)
    {
        var res = await _authService.AuthenticateAsync(dto.Username, dto.Password, dto.UserId, dto.Token);
        SetTokenCookie(res.token, res.expiresAt, res.refreshToken, res.refreshTokenExpiresAt);
        return Ok(new ResponseDto());
    }

    [HttpPost("client/token")]
    public async Task<IActionResult> GetToken(ClientGetTokenDto dto)
    {
        var res = await _authService.ClientTokenRequest(dto.clientId, dto.clientSecret, dto.userId);
        var result = new TokenDto
        {
            Token = res.token,
            expiresAt = res.exp
        };
        var response = new ResponseDto<TokenDto>(Body: result);
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenDto dto)
    {
        if (!Request.Cookies.TryGetValue(AuthConsts.REFRESH_COOKIE_HEADER, out var refreshToken))
            throw new UnauthorizedAccessException("No refresh token provided");

        var res = await _authService.RefreshTokenAsync(dto.UserId, refreshToken);
        SetTokenCookie(res.token, res.expiresAt, res.refreshToken, res.refreshTokenExpiresAt);
        return Ok(new ResponseDto());
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        if(Request.Cookies.TryGetValue(AuthConsts.REFRESH_COOKIE_HEADER, out var refreshToken) &&
           Request.Cookies.TryGetValue(AuthConsts.JWT_COOKIE_HEADER, out var jwtToken))
        {
            await _authService.RevokeRefreshTokenAsync(refreshToken);
        }
        Response.Cookies.Delete(AuthConsts.JWT_COOKIE_HEADER);
        Response.Cookies.Delete(AuthConsts.REFRESH_COOKIE_HEADER, new CookieOptions { Path = RefreshPath });
        return Ok(new ResponseDto());
    }

    [HttpPost("setPassword")]
    public async Task<IActionResult> SetPassword(SetPasswordDto dto)
    {
        await _authService.SetPassword(dto.UserId, dto.RawToken, dto.Password);
        return Ok(new ResponseDto());
    }

    //[HttpGet("/generateNewRsaKey")]
    //[AllowAnonymous]
    //public async Task<IActionResult> Generate()
    //{
    //    await ks.RotateKeysAsync();
    //    return Ok();
    //}

    [HttpGet("ping")]
    public async Task<IActionResult> AuthOnly()
    {
        return Ok();
    }

    private void SetTokenCookie(string token, DateTime expiresAt, string refreshToken, DateTime refreshTokenExpiresAt)
    {
        SetCookie(AuthConsts.JWT_COOKIE_HEADER, token, expiresAt);
        SetCookie(AuthConsts.REFRESH_COOKIE_HEADER, refreshToken, refreshTokenExpiresAt, RefreshPath);
    }

    private void SetCookie(string key, string value, DateTime expiresAt, string path = "/api/")
    {
        var cookieOptions = new CookieOptions
        {
            SameSite = SameSiteMode.None,
            Secure = true,
            HttpOnly = false,
            Expires = expiresAt,
            Path = path
        };

        Response.Cookies.Append(key, value, cookieOptions);
    }
}
