using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer
{
    /// <summary>
    /// Identity server configuration
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Identity server clients
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client // Sample ASP.NET Core MVC Web App client
                {
                    ClientId = "oidcMVCApp",
                    ClientName = "Sample ASP.NET Core MVC Web App",
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},

                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = new List<string> {"https://localhost:5003/signin-oidc"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    RequirePkce = true,
                    AllowPlainTextPkce = false
                },
                new Client // Movie API client
                {
                    ClientId = "movieAPI",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = { "movieAPI" }
                },
                new Client // Movie MVC application client
                {
                    ClientId = "movieMVC",
                    ClientName = "Movie MVC Web application",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,//.Code,
                    RequirePkce = false,
                    //AllowRememberConsent = false,
                    
                    //RedirectUris = new List<string>()
                    //{
                    //    "https://localhost:5001/signin-oidc"
                    //},
                    RedirectUris = { "https://localhost:5003/signin-oidc" },
                    //PostLogoutRedirectUris = new List<string>()
                    //{
                    //    "https://localhost:5001/signout-callback-oidc"
                    //},
                    PostLogoutRedirectUris = { "https://localhost:5003/signout-callback-oidc" },

                    ClientSecrets = new List<Secret>
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowOfflineAccess = true,
                    AllowedScopes = //new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Address
                        , "roles"
                        , "movieMVC"
                        //, "movieAPI"
                        , "api"
                    }
                }
            };
        /// <summary>
        /// API scopes
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("role"),
                new ApiScope("movieMVC", "MVC application"),
                new ApiScope("movieAPI", "Movie API"),
                new ApiScope("api", "Test")
                //, new ApiScope("movieMVC", "Movie MVC Web application")
            };

        /// <summary>
        /// API resources
        /// </summary>
        public static IEnumerable<ApiResource> ApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "Test")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },

                    Scopes = { "api" }
                },
                new ApiResource("movieMVC", "MVC application")
                {
                    ApiSecrets = { new Secret("secret".Sha256()) },

                    Scopes = { "movieMVC" },

                    UserClaims = { "role" }
                }
            };
        }
        /// <summary>
        /// Identity resources
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email(),
                new IdentityResource(
                    "roles",
                    "Your role(s)",
                    new List<string>() { "role" })
            };
        /// <summary>
        /// Test user
        /// </summary>
        public static List<TestUser> TestUsers =>
            new List<TestUser>()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "test",
                    Password = "password",
                    Claims = new List<Claim>
                    {
                        new Claim(JwtClaimTypes.Role, "role")
                    }
                }
            };
    }
}
