using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreStart.MultiTenancy
{
    public class FileTenantProvider : ITenantProvider
    {
        private static IList<Tenant> _tenants;
        private Tenant _tenant;

        public FileTenantProvider(IHttpContextAccessor accessor)
        {
            if (_tenants == null)
            {
                LoadTenants();
            }

            var host = accessor?.HttpContext?.Request?.Host.Value ?? "";
            if (string.IsNullOrEmpty(host))
            {
                _tenant = _tenants.FirstOrDefault(t => t.Id == 1);
            }
            else
            {
                var tenant = _tenants.FirstOrDefault(t => t.Host.ToLower() == host.ToLower());
                if (tenant != null) _tenant = tenant;
            }
        }

        public void LoadTenants()
        {
            _tenants = DeSerializeNonStandardList();
        }

        public IList<Tenant> DeSerializeNonStandardList()
        {
            var fileDir = Path.Combine(Directory.GetCurrentDirectory(), "tenancy.json");
            if (File.Exists(fileDir))
            {
                String json = File.ReadAllText(fileDir);
                var tenants = JsonConvert.DeserializeObject<IEnumerable<Tenant>>(json);
                return tenants.ToList();
            }
            return null;
        }

        public IEnumerable<Tenant> GetAllTenants()
        {
            return _tenants;
        }

        public Tenant GetCurrentTenant()
        {
            return _tenant;
        }
    }
}