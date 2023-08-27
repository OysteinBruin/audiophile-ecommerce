//using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;
using Service.Common.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.AddServiceDefaults();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


// // START----------    AddDefaultOpenApi method
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(options =>
// {
//     options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
//     {
//         Type = SecuritySchemeType.OAuth2,
//         Flows = new OpenApiOAuthFlows
//         {
//             AuthorizationCode = new OpenApiOAuthFlow
//             {
//                 AuthorizationUrl = new Uri("https://localhost:5001/connect/authorize"),
//                 TokenUrl = new Uri("https://localhost:5001/connect/token"),
//                 Scopes = new Dictionary<string, string>
//                 {
//                     {"SampleAPI", "Sample API - full access"}
//                 }
//             },
//         }
//     });
//
//     // Apply Scheme globally
//     options.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme
//             {
//                 Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
//             },
//             new[] { "SampleAPI" }
//         }
//     });
// });
// // END----------    AddDefaultOpenApi method
//
//
//
// // START----------    AddDefaultAuthentication method
// builder.Services.AddAuthentication("Bearer")
//     .AddJwtBearer("Bearer", options =>
//     {
//         options.Authority = "https://localhost:5001"; // Our API app will call to the IdentityServer to get the authority
//
//         options.TokenValidationParameters = new TokenValidationParameters()
//         {
//             ValidateAudience = false, // Validate 
//         };
//     });
// // END----------    AddDefaultAuthentication method
//
//
// // TODO - not used in other project
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

// // START----------    UseServiceDefaults method
// // START----------    UseDefaultOpenApi method
//
// app.UseHttpsRedirection();
//
// app.UseAuthentication();
// app.UseAuthorization();
//
// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI(options =>
//     {
//         options.OAuthUsePkce();
//     });
// }


// END----------    UseDefaultOpenApi method
// END----------    UseServiceDefaults method
app.UseServiceDefaults();
app.MapControllers().RequireAuthorization();

app.Run();
