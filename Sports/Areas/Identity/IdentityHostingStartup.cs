using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sports.Areas.Identity.Data;
using Sports.Models;

[assembly: HostingStartup(typeof(Sports.Areas.Identity.IdentityHostingStartup))]
namespace Sports.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<SportsContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("SportsContextConnection")));

                services.AddDefaultIdentity<SportsUser>()
                    .AddEntityFrameworkStores<SportsContext>();
            });
        }
    }
}