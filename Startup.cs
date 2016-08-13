using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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
            services.AddMvc();
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
