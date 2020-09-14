using IdentityServer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity(Configuration);

            var assembly = GetType().Assembly.GetName().Name;

            services.AddIdentityServer()
                .AddAspNetIdentity<IdentityUser>()
                .AddConfigurationStore(opts =>
                {
                    opts.ConfigureDbContext = x => x.UseSqlite(Configuration["Database:ConnectionString"],
                        sql => sql.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = x => x.UseSqlite(Configuration["Database:ConnectionString"],
                        sql => sql.MigrationsAssembly(assembly));
                })
            //.AddInMemoryIdentityResources(IdentityConfiguration.GetIdentityResources())
            //.AddInMemoryApiResources(IdentityConfiguration.GetApis())
            //.AddInMemoryApiScopes(IdentityConfiguration.GetScopes())
            //.AddInMemoryClients(IdentityConfiguration.GetClients())
            .AddDeveloperSigningCredential();

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHsts();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
