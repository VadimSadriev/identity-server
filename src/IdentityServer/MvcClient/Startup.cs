using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MvcClient
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = "Cookie";
                opts.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookie")
                .AddOpenIdConnect("oidc", opts =>
                {
                    opts.Authority = "https://localhost:5000/";
                    opts.ClientId = "client_id_mvc";
                    opts.ClientSecret = "client_secret_mvc";
                    opts.SaveTokens = true;
                    opts.ResponseType = "code";
                    opts.SignedOutCallbackPath = "/home/index";

                    // configure cookie claim mapping
                    opts.ClaimActions.DeleteClaim("arm");
                    opts.ClaimActions.DeleteClaim("s_hash");
                    opts.ClaimActions.MapUniqueJsonKey("mvc_client_character", "server.character");
                    // maps external claim "name" into http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name
                    opts.ClaimActions.MapJsonKey("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name", "name");

                    // makes additional request  to get other user claims instead of
                    // having big id token.
                    opts.GetClaimsFromUserInfoEndpoint = true;

                    // configure scope
                    opts.Scope.Clear();
                    opts.Scope.Add("openid");
                    opts.Scope.Add("rc.scope");
                    opts.Scope.Add("ApiOne");
                    opts.Scope.Add("offline_access");
                });

            services.AddControllersWithViews();

            services.AddHttpClient();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
