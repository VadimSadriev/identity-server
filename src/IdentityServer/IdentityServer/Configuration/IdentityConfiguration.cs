using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServer.Configuration
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource
                {
                    Name = "rc.scope",
                    UserClaims = { "server.character" }
                }
            };
        }

        public static IEnumerable<ApiResource> GetApis()
        {
            return new List<ApiResource>
            {
                new ApiResource("ApiOne")
            };
        }

        public static IEnumerable<ApiScope> GetScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("ApiOne", new [] { "server.api.characater" }),
                new ApiScope("ApiTwo"),
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "client_id",
                    ClientSecrets = { new Secret("client_secret".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "ApiOne" }
                },
                new Client
                {
                    ClientId = "client_id_mvc",
                    ClientSecrets = { new Secret("client_secret_mvc".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = { "ApiOne", "ApiTwo",
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile,
                        "rc.scope"
                    },
                    RedirectUris = { "https://localhost:5003/signin-oidc" },
                    AllowOfflineAccess = true
                    // puts all the claims in id token
                    //AlwaysIncludeUserClaimsInIdToken = true,
                },
                new Client
                {
                    ClientId = "client_id_js",
                    ClientSecrets = { new Secret("client_secret_js".ToSha256()) },
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedCorsOrigins = { "https://localhost:5004" },
                    RedirectUris = { "https://localhost:5004/home/signin" },
                     AllowedScopes = {
                        "ApiOne",
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId,
                        "rc.scope",
                        "ApiTwo"
                    },
                    AccessTokenLifetime = 1,
                    AllowAccessTokensViaBrowser = true
                }
            };
        }
    }

}
