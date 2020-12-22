using System.Collections.Generic;
using System.Linq;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;

namespace Identity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };

        public static IEnumerable<ApiScope> ApiScopes(IConfiguration configuration)
        {
            var apiScopes = configuration.GetSection("IdentityServer:scopes")
                .GetChildren()
                .Select(c => new ApiScope(c.Value))
                .ToArray();

            return new ApiScope[]
            {
                new ApiScope("scope1"),
                new ApiScope("scope2"),
            };
        }

        public static IEnumerable<Client> Clients(IConfiguration configuration)
        {
            var clients = configuration.GetSection("IdentityServer")
                .GetChildren()
                .Select(c => configuration.GetSection(c.Path).GetConfig())
                .ToArray();


            //var clients=  configuration.GetSection("IdentityServer");

            return new Client[]
            {
                // m2m client credentials flow client
                new Client
                {
                    ClientId = "m2m.client",
                    ClientName = "Client Credentials Client",

                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = {new Secret("511536EF-F270-4058-80CA-1C89C192F69A".Sha256())},

                    AllowedScopes = {"scope1"}
                },

                // interactive client using code flow + pkce
                new Client
                {
                    ClientId = "interactive",

                    AllowedGrantTypes = GrantTypes.Code,

                    RedirectUris = {"https://localhost:44300/signin-oidc"},

                    FrontChannelLogoutUri = "https://localhost:44300/signout-oidc",
                    PostLogoutRedirectUris = {"https://localhost:44300/signout-callback-oidc"},

                    AllowOfflineAccess = true,

                    ClientSecrets = {new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256())},
                    AllowedScopes = {"openid", "profile", "scope2"}
                },
            };
        }
    }
}