using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.Server;

public static class Config
{
    private static List<string> AllIdentityScopes =>
        IdentityResources.Select(s => s.Name).ToList();
    private static List<string> AllApiScopes =>
        ApiScopes.Select(s => s.Name).ToList();
    private static List<string> AllScopes =>
        AllApiScopes.Concat(AllIdentityScopes).ToList();
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            //new ApiScope("basket", "Basket Service"),
            new ApiScope("basket", "Basket API"),
        };
    
    public static IEnumerable<ApiResource> ApiResources()
    {
            return new List<ApiResource>
        {
            new ApiResource("basket", "Basket Service")
        };
    }
    public static IEnumerable<Client> Clients(IConfiguration configuration)
    {
        return new Client[]
        {
            /*new Client
            {
                ClientId = "basketswaggerui",
                ClientName = "Basket Swagger UI",
                AllowedGrantTypes = GrantTypes.Code,
                AllowAccessTokensViaBrowser = true,
                RequirePkce = false,
                RedirectUris = { "http://localhost:5062/swagger/oauth2-redirect.html" },
                PostLogoutRedirectUris = { "http://localhost:5062/swagger/" },

                AllowedScopes =
                {
                    "basket"
                }
            }*/
            
            // // Swagger client
            // new Client
            // {
            //     ClientId = "basketswaggerui",
            //     ClientName = "Basket Swagger UI",
            //     AllowedGrantTypes = GrantTypes.Implicit,
            //     AllowAccessTokensViaBrowser = true,
            //     RequirePkce = false,
            //     //RedirectUris = { $"{configuration["BasketApiClient"]}/swagger/oauth2-redirect.html" },
            //     RedirectUris = {"http://localhost:5103/swagger/oauth2-redirect.html"},
            //     //PostLogoutRedirectUris = { $"{configuration["BasketApiClient"]}/swagger/" },
            //     PostLogoutRedirectUris = { $"{configuration["BasketApiClient"]}/swagger/" },
            //
            //     AllowedScopes =
            //     {
            //         "basket"
            //     },
            //
            //     //ClientSecrets = {new Secret("secret".Sha256())},
            //     
            //
            //     
            //     AllowedCorsOrigins = {"http://localhost:5103"},
            //
            // },
            // Swagger client
            new Client
            {
                ClientId = "basketswaggerui",
                ClientName = "Basket Swagger UI",
                ClientSecrets = {new Secret("secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                //AllowAccessTokensViaBrowser = true,

                RedirectUris = {"http://localhost:5103/swagger/oauth2-redirect.html"},
                AllowedCorsOrigins = {"http://localhost:5103"},
                AllowedScopes =
                {
                    "basket"
                },

                //ClientSecrets = {new Secret("secret".Sha256())},
                

                
                

            },
             //NextJs client
            new Client
            {
                ClientId = "nextjs_web_app",
                ClientName = "NextJs Web App",
                ClientSecrets = {new Secret("secret".Sha256())},  //change me!
                RequireClientSecret = false,
                AllowedGrantTypes =  new[] { GrantType.AuthorizationCode, GrantType.ResourceOwnerPassword },
                    
                AllowOfflineAccess = true,

                //// where to redirect to after login
                RedirectUris = { "http://localhost:3000/api/auth/callback/sample-identity-server" },
                //// where to redirect to after logout
                PostLogoutRedirectUris = { "http://localhost:3000" },
                AllowedCorsOrigins= { "http://localhost:3000" },

                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    "SampleAPI"
                },
            },
            new Client
            {
                ClientId = "interactive.confidential",
                ClientName = "Interactive client (Code with PKCE)",

                RedirectUris = { "https://notused" },
                PostLogoutRedirectUris = { "https://notused" },

                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.CodeAndClientCredentials,
                AllowedScopes = AllScopes,

                AllowOfflineAccess = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RefreshTokenExpiration = TokenExpiration.Sliding
            },
            
        };
    }
}