using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReadingRainbowAPI.DAL;
using Neo4j.Driver;
using System.Net.Http;

namespace ReadingRainbowAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set;}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var neoURI = Configuration["NeoURI"];
            var neoUserName = Configuration["NeoUserName"];
            var neoPW = Configuration["NeoPW"];

            //services.AddDBContext<Neo4jDBContext>(d=>d.GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("Neo4j", "Neo4jPassword")))
            services.AddSingleton(GraphDatabase.Driver("bolt://localhost:7687", AuthTokens.Basic("Neo4j", "Neo4jPassword")));
            // services.AddScoped<IBaseRepository, BaseRepository>();

            services.AddControllers();

            // Add policy for CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddMvcCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll"); // Add this line here or inside if (env.IsDevelopment()) block


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}