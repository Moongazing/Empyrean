using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.EnableEmailAuthenticator;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.EnableOtpAuthenticator;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.Login;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.Refresh;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.Register;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.Revoke;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.VerifyEmailAuthenticator;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.VerifyOtpAuthenticator;
using Moongazing.Empyrean.WebApi.Controllers.Common;
using Moongazing.Kernel.Application.Dtos;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.WebApi.Controllers;

[Route("api/empyrean/authentication")]
[ApiController]
public sealed class AuthenticationController : BaseController
{
    private readonly WebApiConfiguration configuration;

    public AuthenticationController(IConfiguration configuration)
    {
        const string configurationSection = "WebAPIConfiguration";
        this.configuration =
            configuration.GetSection(configurationSection).Get<WebApiConfiguration>()
            ?? throw new NullReferenceException($"\"{configurationSection}\" section cannot found in configuration.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userForLoginDto)
    {
        var loginCommand = new LoginCommand
        {
            UserLoginDto = userForLoginDto,
            IpAddress = GetIpAddress()
        };

        var result = await Sender.Send(loginCommand).ConfigureAwait(false);

        if (result.RefreshToken is not null)
            SetRefreshTokenToCookie(result.RefreshToken);

        return Ok(result.ToHttpResponse());
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto userForRegisterDto)
    {
        var registerCommand = new RegisterCommand
        {
            UserRegisterDto = userForRegisterDto,
            IpAddress = GetIpAddress()
        };

        var result = await Sender.Send(registerCommand).ConfigureAwait(false);

        if (result.RefreshToken is not null)
            SetRefreshTokenToCookie(result.RefreshToken);

        return Created(string.Empty, result);
    }

    [HttpGet("refreshtoken")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshTokenCommand = new RefreshTokenCommand
        {
            RefreshToken = GetRefreshTokenFromCookies(),
            IpAddress = GetIpAddress()
        };

        var result = await Sender.Send(refreshTokenCommand).ConfigureAwait(false);

        if (result.RefreshToken is not null)
            SetRefreshTokenToCookie(result.RefreshToken);

        return Created(string.Empty, result);
    }

    [HttpPut("revoketoken")]
    public async Task<IActionResult> RevokeToken([FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] string? refreshToken)
    {
        var revokeTokenCommand = new RevokeTokenCommand
        {
            Token = refreshToken ?? GetRefreshTokenFromCookies(),
            IpAddress = GetIpAddress()
        };

        var result = await Sender.Send(revokeTokenCommand).ConfigureAwait(false);

        return Ok(result);
    }

    [HttpGet("enable/emailauthenticator")]
    public async Task<IActionResult> EnableEmailAuthenticator()
    {
        var enableEmailAuthenticatorCommand = new EnableEmailAuthenticatorCommand
        {
            UserId = GetUserIdFromRequest(),
            VerifyEmailUrlPrefix = $"{configuration.ApiDomain}/verify/emailauthenticator"
        };

        await Sender.Send(enableEmailAuthenticatorCommand).ConfigureAwait(false);

        return Ok();
    }

    [HttpGet("enable/otpauthenticator")]
    public async Task<IActionResult> EnableOtpAuthenticator()
    {
        var enableOtpAuthenticatorCommand = new EnableOtpAuthenticatorCommand
        {
            UserId = GetUserIdFromRequest()
        };

        var result = await Sender.Send(enableOtpAuthenticatorCommand).ConfigureAwait(false);

        return Ok(result);
    }

    [HttpGet("verify/emailauthenticator")]
    public async Task<IActionResult> VerifyEmailAuthenticator([FromQuery] VerifyEmailAuthenticatorCommand verifyEmailAuthenticatorCommand)
    {
        await Sender.Send(verifyEmailAuthenticatorCommand).ConfigureAwait(false);

        return Ok();
    }

    [HttpPost("verify/otpauthenticator")]
    public async Task<IActionResult> VerifyOtpAuthenticator([FromBody] string authenticatorCode)
    {
        var verifyOtpAuthenticatorCommand = new VerifyOtpAuthenticatorCommand
        {
            UserId = GetUserIdFromRequest(),
            ActivationCode = authenticatorCode
        };

        await Sender.Send(verifyOtpAuthenticatorCommand).ConfigureAwait(false);

        return Ok();
    }

    private string GetRefreshTokenFromCookies() => Request.Cookies["refreshToken"]
                        ?? throw new ArgumentException("Refresh token is not found in request cookies.");


    private void SetRefreshTokenToCookie(RefreshTokenEntity refreshToken)
    {
        CookieOptions cookieOptions = new() { HttpOnly = true, Expires = DateTime.UtcNow.AddDays(7) };
        Response.Cookies.Append(key: "refreshToken", refreshToken.Token, cookieOptions);
    }
}
