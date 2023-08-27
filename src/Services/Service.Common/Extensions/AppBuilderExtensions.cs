using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Service.Common.Extensions;

public static class AppBuilderExtensions
{
    public static WebApplicationBuilder AddServiceDefaults(this WebApplicationBuilder builder)
    {
        // Shared configuration via key vault
        //builder.Configuration.AddKeyVault();

        // Shared app insights configuration
        //builder.Services.AddApplicationInsights(builder.Configuration);

        // Default health checks assume the event bus and self health checks
        //builder.Services.AddDefaultHealthChecks(builder.Configuration);

        // Add the event bus
        //builder.Services.AddEventBus(builder.Configuration);

        builder.Services.AddDefaultAuthentication(builder.Configuration);

        builder.Services.AddDefaultOpenApi(builder.Configuration);

        // Add the accessor
        builder.Services.AddHttpContextAccessor();

        return builder;
    }

    public static WebApplication UseServiceDefaults(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
        }

        var pathBase = app.Configuration["PATH_BASE"];

        if (!string.IsNullOrEmpty(pathBase))
        {
            app.UsePathBase(pathBase);
            app.UseRouting();

            var identitySection = app.Configuration.GetSection("Identity");

            if (identitySection.Exists())
            {
                // We have to add the auth middleware to the pipeline here
                app.UseAuthentication();
                app.UseAuthorization();
            }
        }

        app.UseDefaultOpenApi(app.Configuration);

        //app.MapDefaultHealthChecks();

        return app;
    }

    /*public static async Task<bool> CheckHealthAsync(this WebApplication app)
    {
        app.Logger.LogInformation("Running health checks...");

        // Do a health check on startup, this will throw an exception if any of the checks fail
        var report = await app.Services.GetRequiredService<HealthCheckService>().CheckHealthAsync();

        if (report.Status == HealthStatus.Unhealthy)
        {
            app.Logger.LogCritical("Health checks failed!");
            foreach (var entry in report.Entries)
            {
                if (entry.Value.Status == HealthStatus.Unhealthy)
                {
                    app.Logger.LogCritical("{Check}: {Status}", entry.Key, entry.Value.Status);
                }
            }

            return false;
        }

        return true;
    }*/

    public static IApplicationBuilder UseDefaultOpenApi(this WebApplication app, IConfiguration configuration)
    {
        //var openApiOptions = new OpenApiOptions();
        var openApiSection = configuration.GetSection(OpenApiOptions.SectionName);//.Bind();

        if (!openApiSection.Exists())
        {
            return app;
        }

        //app.UseSwagger();
        //app.UseSwaggerUI(setup =>
        //{
        //    /// {
        //    ///   "OpenApi": {
        //    ///     "Endpoint: {
        //    ///         "Name": 
        //    ///     },
        //    ///     "Auth": {
        //    ///         "ClientId": ..,
        //    ///         "AppName": ..
        //    ///     }
        //    ///   }
        //    /// }

        //    var pathBase = configuration["PATH_BASE"];
        //    var authSection = openApiSection.GetSection("Auth");
        //    var endpointSection = openApiSection.GetRequiredSection("Endpoint");

        //    var swaggerUrl = endpointSection["Url"] ?? $"{(!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty)}/swagger/v1/swagger.json";

        //    setup.SwaggerEndpoint(swaggerUrl, endpointSection.GetRequiredValue("Name"));

        //    if (authSection.Exists())
        //    {
        //        setup.OAuthClientId(authSection.GetRequiredValue("ClientId"));
        //        setup.OAuthAppName(authSection.GetRequiredValue("AppName"));
        //    }
        //});
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.OAuthUsePkce();
        });

        // Add a redirect from the root of the app to the swagger endpoint
        app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

        return app;
    }

    public static IServiceCollection AddDefaultOpenApi(this IServiceCollection services, IConfiguration configuration)
    {
        var openApi = configuration.GetSection("OpenApi");

        if (!openApi.Exists())
        {
            return services;
        }

        services.AddEndpointsApiExplorer();

        return services.AddSwaggerGen(options =>
        {
            // {
            //   "OpenApi": {
            //     "Document": {
            //         "Title": ..
            //         "Version": ..
            //         "Description": ..
            //     }
            //   }
            // }
            var document = openApi.GetRequiredSection("Document");

            var version = document.GetRequiredValue("Version") ?? "v1";

            options.SwaggerDoc(version, new OpenApiInfo
            {
                Title = document.GetRequiredValue("Title"),
                Version = version,
                Description = document.GetRequiredValue("Description")
            });

            var identitySection = configuration.GetSection("Identity");

            if (!identitySection.Exists())
            {
                // No identity section, so no authentication open api definition
                return;
            }

            // {
            //   "Identity": {
            //     "ExternalUrl": "http://identity",
            //     "Scopes": {
            //         "basket": "Basket API"
            //      }
            //    }
            // }

            var identityUrlExternal = identitySection["ExternalUrl"] ?? identitySection.GetRequiredValue("Url");
            var scopes = identitySection.GetRequiredSection("Scopes").GetChildren().ToDictionary(p => p.Key, p => p.Value);
            var authUrl = $"{identityUrlExternal}/connect/authorize";
            var tokenUrl = $"{identityUrlExternal}/connect/token";
            
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        Scopes = new Dictionary<string, string>
                        {
                            {"MySampleAPI", "Sample API - full access"}
                        }
                    },
                }
            });

            // Apply Scheme globally
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                    },
                    new[] { "MySampleAPI" }
                }
            });

        });
    }

    public static IServiceCollection AddDefaultAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // {
        //   "Identity": {
        //     "Url": "http://identity",
        //     "Audience": "basket"
        //    }
        // }

        var identitySection = configuration.GetSection("Identity");

        if (!identitySection.Exists())
        {
            // No identity section, so no authentication
            return services;
        }

        // prevent from mapping "sub" claim to nameidentifier.
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

        services.AddAuthentication().AddJwtBearer("Bearer",options =>
        {
            var identityUrl = identitySection.GetRequiredValue("Url");
            var audience = identitySection.GetRequiredValue("Audience");

            options.Authority = identityUrl;
            options.RequireHttpsMetadata = false;
            options.Audience = audience;
            options.TokenValidationParameters.ValidateAudience = false;
        });

        return services;
    }

    /*public static ConfigurationManager AddKeyVault(this ConfigurationManager configuration)
    {
        // {
        //   "Vault": {
        //     "Name": "myvault",
        //     "TenantId": "mytenantid",
        //     "ClientId": "myclientid",
        //    }
        // }

        var vaultSection = configuration.GetSection("Vault");

        if (!vaultSection.Exists())
        {
            return configuration;
        }

        var credential = new ClientSecretCredential(
            vaultSection.GetRequiredValue("TenantId"),
            vaultSection.GetRequiredValue("ClientId"),
            vaultSection.GetRequiredValue("ClientSecret"));

        var name = vaultSection.GetRequiredValue("Name");

        configuration.AddAzureKeyVault(new Uri($"https://{name}.vault.azure.net/"), credential);

        return configuration;
    }

    public static IServiceCollection AddApplicationInsights(this IServiceCollection services, IConfiguration configuration)
    {
        var appInsightsSection = configuration.GetSection("ApplicationInsights");

        // No instrumentation key, so no application insights
        if (string.IsNullOrEmpty(appInsightsSection["InstrumentationKey"]))
        {
            return services;
        }

        services.AddApplicationInsightsTelemetry(configuration);
        //services.AddApplicationInsightsKubernetesEnricher();
        return services;
    }

    public static IHealthChecksBuilder AddDefaultHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var hcBuilder = services.AddHealthChecks();

        // Health check for the application itself
        hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
        return hcBuilder;
    }

    public static void MapDefaultHealthChecks(this IEndpointRouteBuilder routes)
    {
        routes.MapHealthChecks("/hc", new HealthCheckOptions()
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        routes.MapHealthChecks("/liveness", new HealthCheckOptions
        {
            Predicate = r => r.Name.Contains("self")
        });
    }*/
    
}