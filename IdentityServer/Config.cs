using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    Description = "Allow the service access to your user roles.",
                    UserClaims = new[] { JwtClaimTypes.Role, ClaimTypes.Role },
                    ShowInDiscoveryDocument = true,
                    Required = true,
                    Emphasize = true
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("endalgoapi", "Endalgo API", new []{ "role", "admin", "manager", "user" }),
                new ApiResource("endalgochat", "Endalgo Chat", new []{ "name" }),
                new ApiResource("endalgopayment", "Endalgo Payment")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client",

                    // no interactive user, use the clientid/secret for authentication
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    // secret for authentication
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // scopes that client has access to
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "endalgoapi",
                        "endalgochat"
                    },
                },
                new Client
                {
                    ClientId = "ro.client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "endalgoapi",
                        "endalgochat"
                    },
                },
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    // This will be used to retrieve the access token on the back channel.
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "endalgoapi",
                        "endalgochat"
                    },

                    // This allows requesting refresh tokens for long lived API access
                    AllowOfflineAccess = true
                },
                // JavaScript Client
                new Client
                {
                    ClientId = "js",
                    ClientName = "JavaScript Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,

                    RedirectUris =           { "http://localhost:5003/callback.html" },
                    PostLogoutRedirectUris = { "http://localhost:5003/index.html" },
                    AllowedCorsOrigins =     { "http://localhost:5003" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "endalgoapi",
                        "endalgochat"
                    }
                },

                // Admin
                new Client
                {
                    ClientId = "endalgo.admin",
                    ClientName = "Endalgo Admin",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris =           { "https://admin.endalgo.com/signin-oidc" },
                    PostLogoutRedirectUris = { "https://admin.endalgo.com/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "endalgoapi",
                        "roles"
                    },
                    AllowOfflineAccess = true
                },

                // Admin local
                new Client
                {
                    ClientId = "endalgo.admin local",
                    ClientName = "Endalgo Admin Local",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                    RedirectUris =           { "https://localhost:5004/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:5004/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "endalgoapi",
                        "roles"
                    },
                    AllowOfflineAccess = true
                },

                // Native application client
                new Client
                {
                    ClientId = "com.endalgo.app",
                    ClientName = "Endalgo Native App (Code with PKCE)",

                    RedirectUris = { "com.endalgo.app:/oauthredirect" },
                    PostLogoutRedirectUris = { "com.endalgo.app:/oauthredirect" },
                    AllowedCorsOrigins =     { "com.endalgo.app" },

                    RequireConsent = false,
                    RequireClientSecret = false,

                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "endalgoapi",
                        "endalgochat"
                    },

                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AccessTokenLifetime = 172800,
                },

            };
        }
    }
}
