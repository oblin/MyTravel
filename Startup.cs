using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MyTravel
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();
            // app.UseMvc(config => {
            //     config.MapRoute(
            //         name: "Default",
            //         template: "{Controller}/{action}/{id?}",
            //         defaults: new { controller = "App", action = "Index" }
            //     );
            // });
        }
    }
}
