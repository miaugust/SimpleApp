using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SimpleApp.Areas.Identity.Data;
using SimpleApp.Models;

[assembly: HostingStartup(typeof(SimpleApp.Areas.Identity.IdentityHostingStartup))]
namespace SimpleApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AppDbContext>(options =>
                    options.UseMySql(context.Configuration.GetConnectionString("MySql")));

                services.AddDefaultIdentity<SimpleAppUser>().AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<AppDbContext>();
            });
        }
    }
}