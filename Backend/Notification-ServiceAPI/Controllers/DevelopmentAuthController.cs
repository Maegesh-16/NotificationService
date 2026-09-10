using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notification_ServiceAPI.Authentication;
using Notification_ServiceAPI.Services.Email;

namespace Notification_ServiceAPI.Controllers;

[ApiController]
[Route("api/dev-auth")]
public class DevelopmentAuthController(
    IOptions<JwtSettings> jwtSettings,
    IWelcomeEmailSender welcomeEmailSender) : ControllerBase
{
    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<ActionResult<object>> CreateToken(
        [FromQuery] string role = NotificationServiceRoles.Administrator,
        [FromQuery] string? email = null,
        [FromQuery] string? name = null,
        CancellationToken cancellationToken = default)
    {
        var settings = jwtSettings.Value;
        var now = DateTime.UtcNow;

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, "dev-user"),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, "dev-user"),
            new(ClaimTypes.Name, "dev-user"),
            new(ClaimTypes.Role, role)
        };

        if (!string.IsNullOrWhiteSpace(email))
        {
            claims.Add(new Claim(ClaimTypes.Email, email));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: settings.Issuer,
            audience: settings.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddHours(2),
            signingCredentials: credentials);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        if (!string.IsNullOrWhiteSpace(email))
        {
            await welcomeEmailSender.SendAsync(email, name ?? "Customer", cancellationToken);
        }

        return Ok(new
        {
            accessToken = jwt,
            tokenType = "Bearer",
            expiresAtUtc = now.AddHours(2),
            role
        });
    }
}
