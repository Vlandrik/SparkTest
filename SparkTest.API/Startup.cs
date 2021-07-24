using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SparkTest.API.Configurators;
using SparkTest.DAL.Implementations.DataContext;
using SparkTest.DAL.Implementations.Repository;
using SparkTest.DAL.Interfaces.DataContext;
using SparkTest.DAL.Interfaces.Repository;
using SparkTest.Services.Implementations;
using SparkTest.Services.Interfaces;
using System.Text.Json.Serialization;

namespace SparkTest.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDataContext, DataContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IJWTService, JWTService>();

            services.AddMvcCore(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.EnableEndpointRouting = false;
            });

            services.ConfigureJWT();
            services.AddAuthorization();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddRouting();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
