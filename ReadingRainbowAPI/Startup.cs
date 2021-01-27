using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            var neoCreds = new NeoCredentials();

           services.AddScoped<INeo4jDBContext, Neo4jDBContext>(n=>new Neo4jDBContext(neoCreds.NeoUri, neoCreds.NeoUserName, neoCreds.NeoPassword));
           services.AddAuthorization();

            //This is new code added for swagger as part of configuration 1
            services.AddControllers();
              services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Swagger Demo API",
                        Description = "Demo API for showing swagger",
                        Version = "v1"
                    });
            });

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


            services.AddScoped<BookRepository>(); 
            services.AddScoped<PersonRepository>(); 
            services.AddScoped<GenreRepository>(); 
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

            app.UseRouting();

            app.UseAuthentication();  
            app.UseAuthorization(); 

                        // This code is added to for the configuaration 
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Demp API");
                options.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });



        }
    }
}