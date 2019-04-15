using Ardalis.ListStartupServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using Yocale.eShop.ApplicationCore.Interfaces;
using Yocale.eShop.ApplicationCore.Services;
using Yocale.eShop.Infrastructure.Data;
using Yocale.eShop.Infrastructure.Identity;
using Yocale.eShop.Infrastructure.Logging;
using Yocale.eShop.WebAPI.Interfaces;
using Yocale.eShop.WebAPI.Services;

namespace Yocale.eShop.WebAPI
{
    public class Startup
    {
        private IServiceCollection _services;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //ConfigureCookieSettings(services);

            CreateIdentityIfNotCreated(services);

            services.AddScoped(typeof(IAsyncRepository<>), typeof(EfRepository<>));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICachedProductService, CachedProductService>();
            services.AddScoped<ApplicationCore.Interfaces.IBasketService, ApplicationCore.Services.BasketService>();
            services.AddScoped<Interfaces.IBasketService, Services.BasketService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IProductService, ProductService>();
            services.Configure<ProductSettings>(Configuration);
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
       
            // Add memory cache services
            services.AddMemoryCache();

            services.AddRouting(options =>
            {
                // Replace the type and the name used to refer to it with your own
                // IOutboundParameterTransformer implementation
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
            });

            services.AddMvc(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(
                         new SlugifyParameterTransformer()));

            }
            )
                .AddRazorPagesOptions(options =>
                {
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpContextAccessor();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Yocale eShop API", Version = "v1" });
                c.EnableAnnotations();
            });

            services.Configure<ServiceConfig>(config =>
            {
                config.Services = new List<ServiceDescriptor>(services);

                config.Path = "/allservices";
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });

            _services = services; // used to debug registered services
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Yocale API V1");
                c.RoutePrefix = "docs";
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        public void ConfigureProductionServices(IServiceCollection services)
        {
            services.AddDbContext<EShopContext>(options =>
               options.UseSqlServer(Configuration["ConnectionStrings:EShopConnection"]));

            services.AddDbContext<AppIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection")));

            ConfigureServices(services);
        }

        public void ConfigureDevelopmentServices(IServiceCollection services)
        {
            ConfigureProductionServices(services);
        }

        private static void CreateIdentityIfNotCreated(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var existingUserManager = scope.ServiceProvider
                    .GetService<UserManager<ApplicationUser>>();
                if (existingUserManager == null)
                {
                    services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddDefaultUI(UIFramework.Bootstrap4)
                        .AddEntityFrameworkStores<AppIdentityDbContext>()
                                        .AddDefaultTokenProviders();
                }
            }
        }
    }
}
