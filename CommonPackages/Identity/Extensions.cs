using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Identity
{
     public static class Extensions
        {
            public static Client GetConfig(this IConfigurationSection section)
            {
                var clientConfig = new ClientConfig();
                
                section.Bind(clientConfig);
                var client = new Client
                {
                    ClientId = clientConfig.ClientId,
                    ClientName = clientConfig.ClientName,
                    AllowedGrantTypes = new[] {clientConfig.AllowedGrantTypes},
                    RedirectUris = new[] {clientConfig.RedirectUris},
                    FrontChannelLogoutUri = clientConfig.FrontChannelLogoutUri,
                    PostLogoutRedirectUris = new[] {clientConfig.PostLogoutRedirectUris},
                    AllowOfflineAccess = clientConfig.AllowOfflineAccess,
                    ClientSecrets = new[] {new Secret(clientConfig.ClientSecrets.Sha256())},
                    AllowedScopes = clientConfig.AllowedScopes.Split(",")
                };
                return client;
            }

            public static void AddCustomIdentityServer(this IServiceCollection services , IConfiguration configuration , string assembly)
            {
                
                
                services.AddIdentityServer(options =>
                    {
                        options.Events.RaiseErrorEvents = true;
                        options.Events.RaiseInformationEvents = true;
                        options.Events.RaiseFailureEvents = true;
                        options.Events.RaiseSuccessEvents = true;

                        // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                        options.EmitStaticAudienceClaim = true;
                    })
                    .AddAspNetIdentity<IdentityUser>()
                    .AddTestUsers(TestUsers.Users)
                    // this adds the config data from DB (clients, resources, CORS)
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                sql => sql.MigrationsAssembly(assembly));
                    })
                    // this adds the operational data from DB (codes, tokens, consents)
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                sql => sql.MigrationsAssembly(assembly));

                        // this enables automatic token cleanup. this is optional.
                        options.EnableTokenCleanup = true;
                        options.TokenCleanupInterval = 30;
                    })
                    .AddDeveloperSigningCredential();
            }
        }
}



// nuget push Idsv4Package.1.3.0.nupkg oy2ln3eyzm7ikjepf27azzvvvlupudeelobalukmzhqh3i -Source https://api.nuget.org/v3/index.json