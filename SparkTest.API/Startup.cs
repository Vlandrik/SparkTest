using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using SparkTest.API.Configurators;
using SparkTest.DAL.Implementations.DataContext;
using SparkTest.DAL.Implementations.Repository;
using SparkTest.DAL.Interfaces.DataContext;
using SparkTest.DAL.Interfaces.Repository;
using SparkTest.MessageBroker.Implementations;
using SparkTest.MessageBroker.Interfaces;
using SparkTest.Services.Implementations;
using SparkTest.Services.Interfaces;
using SparkTest.Services.Settings;
using System.Text.Json.Serialization;

namespace SparkTest.API
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<UserInfo>(_configuration.GetSection("UserInfo"));

            services.AddTransient<IDataContext, DataContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IJWTService, JWTService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IMessageBrokerService, MessageBrokerService>();

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

            app.UseSerilogRequestLogging();

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
