using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AquAnalyzerAPI.Auth
{
    public static class AuthorizationPolicies
    {
        public static void AddPolicies(IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                // Policy for Analysts
                options.AddPolicy("MustBeAnalyst", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireClaim("Role", "Analyst"));

                // Policy for Visual Designers
                options.AddPolicy("MustBeVisualDesigner", policy =>
                    policy.RequireAuthenticatedUser()
                          .RequireClaim("Role", "VisualDesigner"));
            });
        }
    }
}
