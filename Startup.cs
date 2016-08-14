using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyTravel.Models;
using MyTravel.Services;
using MyTravel.ViewModels;
using Newtonsoft.Json.Serialization;

namespace MyTravel
{
    public class Startup
    {
        private IHostingEnvironment _env;
        private IConfigurationRoot _config;

        public Startup(IHostingEnvironment env)
        {
            _env = env;
            var builder = new ConfigurationBuilder()
                              .SetBasePath(_env.ContentRootPath)
                              .AddJsonFile("config.json")
                              .AddEnvironmentVariables();
            _config = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_env.IsDevelopment())
                services.AddTransient<IMailService, DebugMailService>();
            services.AddSingleton(_config);

            services.AddDbContext<TravelContext>();
            services.AddScoped<ITravelRepository, TravelRepository>();
            services.AddTransient<TravelContextSeedData>();
            services.AddTransient<GeoCoordsService>();
            services.AddLogging();
            services.AddIdentity<TravelUser, IdentityRole>(config => {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
                config.Cookies.ApplicationCookie.LoginPath = "/Auth/Login";
                config.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents()
                {
                    OnRedirectToLogin = async ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api")
                            && ctx.Response.StatusCode == 200)
                        {
                            ctx.Response.StatusCode = 401;                            
                        }
                        else 
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        await Task.Yield();
                    }
                };
            }).AddEntityFrameworkStores<TravelContext>();
            
            services.AddMvc(config => {
                if (_env.IsProduction())
                {
                    config.Filters.Add(new RequireHttpsAttribute());
                }
            });
                    // .AddJsonOptions(config => config.SerializerSettings.ContractResolver
                    //     = new CamelCasePropertyNamesContractResolver());
        }

        public void Configure(IApplicationBuilder app, 
            ILoggerFactory loggerFactory,
            TravelContextSeedData seed)
        {
            Mapper.Initialize(config => {
                config.CreateMap<TripViewModel, Trip>().ReverseMap();
                config.CreateMap<StopViewModel, Stop>().ReverseMap();
            });
            
            if (_env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddConsole(LogLevel.Information);
            }
            else 
            {
                loggerFactory.AddConsole(LogLevel.Error);
            }

            app.UseStaticFiles();
            app.UseIdentity();
            
            app.UseMvc(config => {
                config.MapRoute(
                    name: "Default",
                    template: "{Controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                );
            });

            seed.EnsureSeedData().Wait();
        }
    }
}
