using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace OidcClientDemoApplication
{
    public class Startup
    {

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration config)
        {
            Environment = environment;
            Configuration = config;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Prevent WS-Federation claim names being written to tokens
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                // Use the strongest setting in production, which also enables HTTP on developer workstations
                options.Cookie.SameSite = SameSiteMode.Strict;
            })
            .AddOpenIdConnect(options =>
            {

                // Use the same settings for temporary cookies
                options.NonceCookie.SameSite = SameSiteMode.Strict;
                options.CorrelationCookie.SameSite = SameSiteMode.Strict;

                // Set the main OpenID Connect settings
                options.Authority = Configuration.GetValue<string>("OpenIdConnect:Issuer");
                options.ClaimsIssuer = "Authgear";

                options.ClientId = Configuration.GetValue<string>("OpenIdConnect:ClientId");
                options.ClientSecret = Configuration.GetValue<string>("OpenIdConnect:ClientSecret");

                options.ResponseType = OpenIdConnectResponseType.Code;

                var scopeString = Configuration.GetValue<string>("OpenIDConnect:Scope");
                options.Scope.Clear();
                scopeString.Split(" ", StringSplitOptions.TrimEntries).ToList().ForEach(scope =>
                {
                    options.Scope.Add(scope);
                });

                // If required, override the issuer and audience used to validate ID tokens
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = options.Authority,
                    ValidAudience = options.ClientId
                };

                // This example gets user information for display from the user info endpoint
                options.GetClaimsFromUserInfoEndpoint = true;

                // Handle the post logout redirect URI
                options.Events.OnRedirectToIdentityProviderForSignOut = (context) =>
                {
                    context.ProtocolMessage.PostLogoutRedirectUri = Configuration.GetValue<string>("OpenIdConnect:PostLogoutRedirectUri");
                    return Task.CompletedTask;
                };

                // Save tokens issued to encrypted cookies
                options.SaveTokens = true;

                // Set this in developer setups if the OpenID Provider uses plain HTTP
                options.RequireHttpsMetadata = false;
            });

            services.AddAuthorization();
            services.AddRazorPages();

            // Add this app's types to dependency injection
            services.AddSingleton<TokenClient>();
        }
    }
}
