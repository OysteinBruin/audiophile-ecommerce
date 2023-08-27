

// using Service.Common.Extensions;
//
// var builder = WebApplication.CreateBuilder(args);
//
// // Add services to the container.
// builder.AddServiceDefaults();
// builder.Services.AddControllers();
// // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// //builder.Services.AddEndpointsApiExplorer();
// //builder.Services.AddSwaggerGen();
//
// var app = builder.Build();
//
// // Configure the HTTP request pipeline.
// //if (app.Environment.IsDevelopment())
// //{
// //    app.UseSwagger();
// //    app.UseSwaggerUI();
// //}
// app.UseServiceDefaults();
// app.UseHttpsRedirection();
//
// //app.UseAuthorization();
//
// app.MapControllers();
//
// app.Run();

using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    
    string elasticUri = builder.Configuration["ElasticConfiguration:Uri"];
    string appName = builder.Configuration["ApplicationName"];
    Log.Information("Elastic Uri: {ElasticUri}, application name: {AppName}", elasticUri, appName);
    builder.Logging.ClearProviders();
    // Add services to the container.
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}")
            .WriteTo.Elasticsearch(
                new ElasticsearchSinkOptions(new Uri(elasticUri))    
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    IndexFormat = $"{appName}-logs-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-") ?? "development"}-{DateTime.UtcNow:yyyy-MM}",
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
            .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
            .ReadFrom.Configuration(context.Configuration);
    });

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri("http://localhost:5105/connect/authorize"),
                    TokenUrl = new Uri("http://localhost:5105/connect/token"),
                    Scopes = new Dictionary<string, string>
                    {
                        {"basket", "Sample API - full access"}
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
                new[] { "basket" }
            }
        });
    });

    builder.Services.AddAuthentication("Bearer")
        .AddJwtBearer("Bearer", options =>
        {
            var identityUrl = "http://localhost:5105"; // Our API app will call to the IdentityServer to get the authority
            var audience = "basket";
            options.Authority = identityUrl;
            options.RequireHttpsMetadata = false;
            options.Audience = audience;
            // options.TokenValidationParameters.ValidateAudience = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false, // Validate 
            };
        });
    
    // Uncomment 15.08.2023
    // builder.Services.AddAuthorization(options =>
    // {
    //     // Add a policy called "ApiScope" which is required our users are authenticated and have the API Scope Claim call WebAPI 
    //     options.AddPolicy("ApiScope", policy =>
    //     {
    //         policy.RequireAuthenticatedUser();
    //         policy.RequireClaim("scope", "WebAPI");
    //     });
    // });

    var app = builder.Build();

    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            //options.OAuthClientId("basketswaggerui");
            //options.OAuthAppName("Basket Swagger UI");
            options.OAuthUsePkce();
        });
    }

    app.MapControllers().RequireAuthorization();
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

    app.Run();
}
catch (Exception ex) when (
    // https://github.com/dotnet/runtime/issues/60600
    ex.GetType().Name is not "StopTheHostException"
    // HostAbortedException was added in .NET 7, but since we target .NET 6 we
    // need to do it this way until we target .NET 8
    && ex.GetType().Name is not "HostAbortedException"
)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}

public partial class Program { }