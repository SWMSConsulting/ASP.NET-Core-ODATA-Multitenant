using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreStart.MultiTenancy
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Host { get; set; }
        public string ConnectionString { get; set; }
    }
}
