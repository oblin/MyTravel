using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyTravel.Services;

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
            // if (_env.IsDevelopment())
                services.AddTransient<IMailService, DebugMailService>();
            services.AddSingleton(_config);
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            // if (_env.IsEnvironment("Development"))
                app.UseDeveloperExceptionPage();
            
            app.UseStaticFiles();
            app.UseMvc(config => {
                config.MapRoute(
                    name: "Default",
                    template: "{Controller}/{action}/{id?}",
                    defaults: new { controller = "App", action = "Index" }
                );
            });
        }
    }
}
