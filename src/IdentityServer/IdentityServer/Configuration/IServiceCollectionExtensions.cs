using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Configuration
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentity(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opts =>
            {
                opts.UseInMemoryDatabase("Memory");
            });

            services.AddIdentity<IdentityUser, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 4;
                opts.Password.RequireDigit = false;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(opts =>
            {
                opts.Cookie.Name = "IdentityServer.Cookie";
                opts.LoginPath = "/Auth/Login";
            });

            return services;
        }
    }
}
