using AspNetCoreStart.MultiTenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNetCoreStart.Context
{
    public class ApplicationDbContext : DbContext
    {
        private readonly Tenant _tenant;
        private readonly IConfiguration _configuration;
        private readonly string _userId;

        public DbSet<Todo> Todos { get; set; }


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, Tenant tenant) : base(options)
        {
            _tenant = tenant;
            _configuration = configuration;
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IConfiguration configuration,
            ITenantProvider tenantProvider)
            : base(options)
        {
            _configuration = configuration;
            _tenant = tenantProvider.GetCurrentTenant();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = _configuration.GetConnectionString("ConnectionStringTemplate").Replace("{tenant}", _tenant.ConnectionString);
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
