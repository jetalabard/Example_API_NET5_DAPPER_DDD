using System;
using Support.Application.ApplicationInfos;
using Common.Infrastructure.Repository;
using Support.Infrastructure;
using Support.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Common.Infrastructure.Mails;
using Microsoft.AspNetCore.Http.Features;
using Support.Application.Mail;
using UserAccess.Application.Users;
using UserAccess.Infrastructure.Repository;
using UserAccess.Infrastructure;
using UserAccess.Application.Rfas;
using Serilog;
using Microsoft.Extensions.Logging;
using Support.Application.ApplicationInfos.Queries;
using UserAccess.Application.Users.Commands;
using UserAccess.Application.Queries;
using MediatR;
using UserAccess.Application.Rfas.Commands;
using System.Reflection;
using Common.Application.Queries;
using Common.Infrastructure;
using System.Data;
using System.Data.SqlClient;

namespace Framatome.SupplierManagement.API
{
    public class Startup
    {
        private const string ExampleConnectionString = "ConnectionStrings:Example";

        private ILogger<Startup> _logger;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddControllers();

            services.AddTransient<Func<IDbConnection>>(s => () => new SqlConnection(Configuration[ExampleConnectionString]));
            services.AddScoped<IDbQueryHelper, DapperQueryHelper>();

            services.AddDbContext<SupportContext>(opt =>
            {
                opt.UseSqlServer(Configuration[ExampleConnectionString]);
                opt.ReplaceService<IValueConverterSelector, CustomConverterSelector>();
            });

            services.AddDbContext<UserAccessContext>(opt =>
            {
                opt.UseSqlServer(Configuration[ExampleConnectionString]);
                opt.ReplaceService<IValueConverterSelector, CustomConverterSelector>();
            });

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRfaRepository, RfaRepository>();
            services.AddScoped<IApplicationInfoRepository, ApplicationInfoRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IEmailSenderService, EmailSenderService>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddMediatR(
                            typeof(ApplicationInfoQueryHandler).GetTypeInfo().Assembly,
                            typeof(UserCommandHandler).GetTypeInfo().Assembly,
                            typeof(UserQueryHandler).GetTypeInfo().Assembly,
                            typeof(RfaCommandHandler).GetTypeInfo().Assembly,
                            typeof(RfaQueryHandler).GetTypeInfo().Assembly
                            );

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(x => x.FullName);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exemple.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSerilogRequestLogging();
            _logger = loggerFactory.CreateLogger<Startup>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Example.API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            UpdateDatabase(app);
        }

        private void UpdateDatabase(IApplicationBuilder app)
        {
            try
            {
                using var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
                using var supportContext = serviceScope.ServiceProvider.GetService<SupportContext>();
                supportContext.Database.Migrate();

                using var userAccessContext = serviceScope.ServiceProvider.GetService<UserAccessContext>();
                userAccessContext.Database.Migrate();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
        }
    }
}
