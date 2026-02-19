using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Qsa.Application.Common.Interfaces;
using Qsa.Infrastructure.Auth;
using Qsa.Infrastructure.Surveys;
using System.Text;

namespace Qsa.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddQsaInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DevAuthOptions>(configuration.GetSection(DevAuthOptions.SectionName));
        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddSingleton<IUserStore, InMemoryUserStore>();
        services.AddSingleton<ITokenService, JwtTokenService>();
        services.AddSingleton<ISurveyRepository, InMemorySurveyRepository>();
        services.AddSingleton<ISurveyAssignmentAuthorizer, SurveyAssignmentAuthorizer>();
        services.AddSingleton<IChecklistProvider, InMemoryChecklistProvider>();
        services.AddSingleton<ISurveyResponseStore, InMemorySurveyResponseStore>();
        services.AddSingleton<ISurveyLifecycle, InMemorySurveyLifecycle>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContext, UserContext>();

        var jwtSection = configuration.GetSection(JwtOptions.SectionName);
        var signingKey = jwtSection["SigningKey"] ?? "dev-signing-key-min-32-chars-long-for-hs256";
        var issuer = jwtSection["Issuer"] ?? "Qsa.Dev";
        var audience = jwtSection["Audience"] ?? "Qsa.Client";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(2),
                    NameClaimType = "email",
                    RoleClaimType = "role"
                };
            });

        services.AddAuthorizationBuilder()
            .AddPolicy("VPOnly", p => p.RequireRole("VP"))
            .AddPolicy("ManagerOnly", p => p.RequireRole("Manager"))
            .AddPolicy("SurveyorOnly", p => p.RequireRole("Surveyor"));

        return services;
    }
}
