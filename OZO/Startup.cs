using System.IO.MemoryMappedFiles;
using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace OZO
{
    public class Startup
    {
         public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
            {
            Configuration = configuration;
            }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(); 
            var appSection=Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSection);

            services.AddDbContext<Models.PI09Context>(options => 
                                                  options.UseSqlServer(
                                                            Configuration.GetConnectionString("OZO")
                                                                         .Replace("sifra", Configuration["OZOSqlPassword"])
                              ));    

        }




        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Poslovi",
                    "{controller:regex(^(Poslovi)$)}/Page{page}/Sort{sort:int}/ASC-{ascending:bool}", //samo za mjesto ali može za drugo!!
                    new { action = "Index" }
                    );
                endpoints.MapControllerRoute(
                    name:"default",
                    pattern:"{controller=Home}/{action=Index}/{id?}"
                                  
                    
                );
            });
        }
    }
}
