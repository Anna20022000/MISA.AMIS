using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MISA.Fresher.Core.Interfaces.Service;
using MISA.Fresher.Core.Services;
using MISA.Fresher.Core.Interfaces.Infrastructure;
using MISA.Fresher.Infrastructure.Repository;
using MISA.Fresher.Core.Exceptions;

namespace MISA.Fresher.Api
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyCorsPolicy", policy => policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });

            services.AddControllers(options =>
            { 
                // add the action filter to the filters collection:
                options.Filters.Add<MISAResponseExceptionFilter>();
            });

            //Config DI

            services.AddTransient(typeof(IBaseRepository<>),typeof(BaseRepository<>));
            services.AddTransient(typeof(IBaseService<>),typeof(BaseService<>));

            services.AddTransient(typeof(IEmployeeService), typeof(EmployeeService));
            services.AddTransient(typeof(IEmployeeRepository), typeof(EmployeeRepository));



            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MISA.Fresher.Api", Version = "v1" });
            });

            services.AddControllers().AddJsonOptions(jsonOptions =>
            {
                jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MISA.Fresher.Api v1"));
            }
            app.UseCors("AllowAnyCorsPolicy");

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
