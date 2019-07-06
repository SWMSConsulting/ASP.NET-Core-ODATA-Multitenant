
using AspNetCoreStart.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreStart.MultiTenancy
{
    public static class StartupExtension
    {
        public static void EnsureMigrationsRun(this IApplicationBuilder app, IConfiguration configuration)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContextFactory = serviceScope.ServiceProvider.GetService<IDbContextFactory>();
                var allTenants = serviceScope.ServiceProvider.GetService<ITenantProvider>().GetAllTenants();
                foreach (var tenant in allTenants)
                {
                    var context = dbContextFactory.CreateDbContext(tenant, configuration);
                    context.Database.Migrate();
                }
            }
        }
    }
}
