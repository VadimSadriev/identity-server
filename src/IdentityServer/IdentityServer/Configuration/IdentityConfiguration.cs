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
                new IdentityResources.Profile()
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
                new ApiScope("ApiOne"),
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
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile
                    },
                    RedirectUris = { "https://localhost:5003/signin-oidc" }
                }
            };
        }
    }

}
