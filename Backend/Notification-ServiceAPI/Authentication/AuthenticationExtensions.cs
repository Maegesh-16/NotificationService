using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Notification_ServiceAPI.Authentication;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddNotificationServiceAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

        var jwtSettings = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
            ?? throw new InvalidOperationException("JWT settings were not found.");

        if (string.IsNullOrWhiteSpace(jwtSettings.SecretKey) || jwtSettings.SecretKey.Length < 32)
        {
            throw new InvalidOperationException("JWT secret key must be at least 32 characters long.");
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey));

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = true;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = jwtSettings.ValidateIssuer,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = jwtSettings.ValidateAudience,
                    ValidAudience = jwtSettings.Audience,
                    ValidateLifetime = jwtSettings.ValidateLifetime,
                    ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.FromMinutes(jwtSettings.ClockSkewMinutes)
                };
            });

        services.AddAuthorization(options =>
        {
            options.AddPolicy(NotificationServicePolicies.NotificationRead, policy =>
                policy.RequireAuthenticatedUser().RequireRole(
                    NotificationServiceRoles.Customer,
                    NotificationServiceRoles.Agent,
                    NotificationServiceRoles.ClaimsOfficer,
                    NotificationServiceRoles.CustomerSupport,
                    NotificationServiceRoles.Administrator));

            options.AddPolicy(NotificationServicePolicies.NotificationSend, policy =>
                policy.RequireAuthenticatedUser().RequireRole(
                    NotificationServiceRoles.ClaimsOfficer,
                    NotificationServiceRoles.CustomerSupport,
                    NotificationServiceRoles.Administrator));

            options.AddPolicy(NotificationServicePolicies.NotificationAdmin, policy =>
                policy.RequireAuthenticatedUser().RequireRole(NotificationServiceRoles.Administrator));
        });

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT Bearer token. Example: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}