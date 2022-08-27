using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Messenger.DataAccess.Repositories;
using Messenger.BusinessLogic.Services.Interfaces;
using Messenger.BusinessLogic.Services;
using Messenger.DataAccess.Connection.Interfaces;
using Messenger.DataAccess.Connection;
using Messenger.PresentationLogic.Hubs;
using Messenger.DataAccess.Repositories.Interfaces;
using Messenger.DataAccess.Infrastructure.Interfaces;
using Messenger.DataAccess.Infrastructure;

namespace Messenger.PresentationLogic
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
            services.AddMvc();

            services.AddSignalR();

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Messenger HTTP API",
                    Version = "v1",
                    Description = "The Messenger Service HTTP API"
                });
            });

            services.AddAutoMapper(typeof(Program));

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMessageRepository, MessageRepository>();
            services.AddTransient<IMessageService, MessageService>();

            services.AddSingleton<IStoredProcedureManager, StoredProcedureManager>();

            services.AddScoped<IDbConnectionWrapper, DbConnectionWrapper>(provider => new DbConnectionWrapper(Configuration["ConnectionString"]));

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .WithOrigins()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger()
            .UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint($"{Configuration["PathBase"]}/swagger/v1/swagger.json", "Messenger.API V1");
                setup.OAuthClientId("messengerswaggerui");
                setup.OAuthAppName("Messenger Swagger UI");
            });

            app.UseCookiePolicy();

            app.UseCors("CorsPolicy");

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapHub<NotificationHub>("/notification");
            });
        }
    }
}