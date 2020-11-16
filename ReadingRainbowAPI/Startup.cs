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
using Neo4jClient;
using System.Net.Http;
using ReadingRainbowAPI.Models;
using ReadingRainbowAPI.Controllers;
using ReadingRainbowAPI.Middleware;

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
           var neoUri = "http://localhost:7474";
           var neoUserName = "Neo4j";
           var neoPassword = "abc123";

           services.AddScoped<INeo4jDBContext, Neo4jDBContext>(n=>new Neo4jDBContext(neoUri,neoUserName, neoPassword));
           services.AddAuthorization();

            // Add policy for CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddScoped<BookRepository>(); 
            services.AddScoped<PersonRepository>(); 
            services.AddTokenAuthentication(Configuration);
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

            app.UseAuthentication();  
            app.UseAuthorization(); 
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}