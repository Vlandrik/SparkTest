using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SparkTest.DAL.Implementations.DataContext;
using SparkTest.DAL.Implementations.Repository;
using SparkTest.DAL.Interfaces.DataContext;
using SparkTest.DAL.Interfaces.Repository;

namespace SparkTest.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IDataContext, DataContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello World!");
                });
            });
        }
    }
}
