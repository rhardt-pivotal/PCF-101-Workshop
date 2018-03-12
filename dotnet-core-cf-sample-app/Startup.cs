using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using springmusicdotnetcore.Models;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.Extensions.Configuration.CloudFoundry;
using Steeltoe.CloudFoundry.Connector.MySql.EFCore;
using Steeltoe.CloudFoundry.Connector;
namespace spring_music_dotnet_core
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.ConfigureCloudFoundryOptions(Configuration);
            services.AddDbContext<AlbumContext>(opt => {
                var info = Configuration.GetSingletonServiceInfo<MySqlServiceInfo>();
                Console.Write(info);
                if (info == null) {
                    Console.Write("Using In-Memory");
                    opt.UseInMemoryDatabase("Albums");    
                }
                else {
                    Console.Write("Using MySQL");
                    opt.UseMySql(Configuration);

                }
            });
            services.AddMvc();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseMvc();


        }
    }
}
