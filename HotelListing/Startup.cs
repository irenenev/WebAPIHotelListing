using AspNetCoreRateLimit;
using HotelListing.Configurations;
using HotelListing.Data;
using HotelListing.IRepository;
using HotelListing.Repository;
using HotelListing.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;

namespace HotelListing
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
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection")));

            //for request caching 
            services.AddMemoryCache();
            //for limiting request
            services.ConfigureRateLimiting(); //from ServiceExtension
            services.AddHttpContextAccessor(); //access to actual controller
            //for caching
            services.ConfigureHttpCacheHeaders();
            //for Identity
            services.AddAuthentication();
            //methods from ServiceExtensions.cs
            services.ConfigureIdentity();
            //configuring Data Protection
            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@"\\C:\Users\***\AppData\Local\ASP.NET\DataProtection-Keys"))
            services.ConfigureJWT(Configuration);
            services.AddCors(options=> 
            {
                options.AddPolicy("AllowOrigin", builder =>
                    builder.AllowAnyOrigin() 
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            
            services.AddAutoMapper(typeof(MapperInitializer));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListing", Version = "v1" });
            });
            services.AddControllers(/*config => 
            {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120
                });
            }*/).AddNewtonsoftJson(op =>
                op.SerializerSettings.ReferenceLoopHandling =
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //for version control from ServiceExtensions
            services.ConfigureVersioning();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListing v1"));
            }
            app.ConfigureExceptionHandler(); //from ServiceExtensions
            
            app.UseHttpsRedirection();

            app.UseCors("AllowOrigin");

            app.UseResponseCaching();

            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting(); 

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
