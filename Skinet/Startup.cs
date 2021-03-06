using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Skinet.Core.Interfaces;
using Skinet.Errors;
using Skinet.Extensions;
using Skinet.Helpers;
using Skinet.Infrastructure.Data;
using Skinet.Middleware;
using StackExchange.Redis;

namespace Skinet
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfiels));
            services.AddControllers();
            services.AddApplicationServices();

            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                //var config = ConfigurationOptions.Parse(_configuration.GetConnectionString("Redis"), true);
                //return ConnectionMultiplexer.Connect(config);
                //return ConnectionMultiplexer.Connect("localhost");
                return ConnectionMultiplexer.Connect(_configuration.GetValue<string>("Redis"));
            });

            //services.AddDbContext<StoreContext>(x => x.UseSqlite(_configuration.GetConnectionString("SqliteConnection")));
            services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("SqlServerConnection"));
            });

            services.AddSwaggerDocumentation();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
           
            //app.UseDeveloperExceptionPage();
             
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
