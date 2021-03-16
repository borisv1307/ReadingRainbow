using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Identity;
using ReadingRainbowAPI.Models;
using System;
using ReadingRainbowAPI.Mapping;
using ReadingRainbowAPI.DAL;
using ReadingRainbowAPI.Middleware;
using AutoMapper;
using ReadingRainbowAPI.Shared;

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

            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            var neoCreds = new NeoCredentials(config);

           services.AddSingleton<INeo4jDBContext, Neo4jDBContext>(n=>new Neo4jDBContext(neoCreds.NeoUri, neoCreds.NeoUserName, neoCreds.NeoPassword));
           services.AddAuthorization();

            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Add policy for CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                    });
            });

            services.AddScoped<IEmailHelper, EmailHelper>(e=>new EmailHelper(config));
            services.AddScoped<ITokenClass, TokenClass>(t=>new TokenClass(config));
            services.AddScoped<BookRepository>(); 
            services.AddScoped<PersonRepository>(); 
            services.AddScoped<GenreRepository>(); 
            services.AddScoped<FriendRepository>(); 

            services.AddTokenAuthentication(Configuration);
            services.AddMvcCore();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll"); // Add this line here or inside if (env.IsDevelopment()) block


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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