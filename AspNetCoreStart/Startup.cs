using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreStart.Context;
using AspNetCoreStart.Messaging;
using AspNetCoreStart.MultiTenancy;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AspNetCoreStart
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOData();
            services.AddODataQueryFilter();

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("*");
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<ITenantProvider, FileTenantProvider>();
            services.AddSingleton<IMessageBroadcast, MessageBroadcastEventGrid>();


            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Database")));

            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
            }).AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://login.microsoftonline.com/288ad4ba-39f6-4fce-87f5-5b08b5333484/v2.0";
                options.Audience = "a764cdbb-8a8f-4c09-82c9-27d70dbf51c6";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Clock skew compensates for server time drift.
                    // We recommend 5 minutes or less:
                    ClockSkew = TimeSpan.FromMinutes(5),
                    // Specify the key used to sign the token:
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is super secret")),
                    ValidateIssuerSigningKey = true,
                    RequireSignedTokens = true,
                    // Ensure the token hasn't expired:
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    // Ensure the token audience matches our audience value (default true):
                    ValidateAudience = true,
                    ValidAudience = "a764cdbb-8a8f-4c09-82c9-27d70dbf51c6",
                    // Ensure the token was issued by a trusted authorization server (default true):
                    ValidateIssuer = true,
                    ValidIssuer = "https://login.microsoftonline.com/288ad4ba-39f6-4fce-87f5-5b08b5333484/v2.0"
                };
            });

            services.AddTransient<IDbContextFactory, DbContextFactory>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(MyAllowSpecificOrigins);
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMvc(routeBuilder =>
            {
                routeBuilder.Select().Expand().Filter().OrderBy().MaxTop(null).Count();
                routeBuilder.MapODataServiceRoute("odata", "odata", AppModelBuilder.GetEdmModel(app.ApplicationServices));
            });

            app.EnsureMigrationsRun(Configuration);

        }
    }
}
