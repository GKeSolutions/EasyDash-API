using API;
using Core.Interface;
using Core.Service;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notification;
using System.Text.Json.Serialization;

namespace EasyDash_API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public WebApplicationBuilder Builder { get; private set; }

        string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            Builder = WebApplication.CreateBuilder();

            Builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    policy =>
                    {
                        policy.WithOrigins("http://3e-dev-wapi:5010", "https://3e-dev-wapi:5010", "http://localhost:4200")
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS")
                        .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
                    });

            });

            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            Builder.Services.AddSingleton(emailConfig);
            Builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            Builder.Services.AddEndpointsApiExplorer();
            Builder.Services.AddSwaggerGen();
            //builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            //   .AddNegotiate();

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = options.DefaultPolicy;
            //});

            Builder.Services.AddTransient<IDashboardService, DashboardService>();
            Builder.Services.AddTransient<ILookupService, LookupService>();
            Builder.Services.AddTransient<IAnalyticsService, AnalyticsService>();
            Builder.Services.AddTransient<INotificationService, NotificationService>();
            Builder.Services.AddTransient<IScheduledNotificationService, ScheduledNotificationService>();
            Builder.Services.AddTransient<ISchedulerService, SchedulerService>();
            Builder.Services.AddTransient<IMissingTimeService, MissingTimeService>();
            Builder.Services.AddTransient<IJobService, JobService>();
            Builder.Services.AddTransient<IReassignService, ReassignService>();
            Builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            Builder.Services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            Builder.Services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<ExceptionMiddleware>), typeof(Logger<ExceptionMiddleware>)));

            Builder.Services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true,
                    DisableGlobalLocks = true
                }));
            Builder.Services.AddHangfireServer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            var application = Builder.Build();
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");
            if (env.IsDevelopment())
            {
                application.UseSwagger();
                application.UseSwaggerUI();
            }
            application.UseMiddleware<ExceptionMiddleware>();
            application.UseHttpsRedirection();
            application.UseRouting();
            application.UseCors(builder =>
            {
                builder
                      .WithOrigins("http://3e-dev-wapi:5010", "https://3e-dev-wapi:5010", "http://localhost:4200")
                      .SetIsOriginAllowedToAllowWildcardSubdomains()
                      .AllowAnyHeader()
                      .AllowCredentials()
                      .WithMethods("GET", "PUT", "POST", "DELETE", "OPTIONS")
                      .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));

            });

            application.UseAuthentication();
            application.UseAuthorization();

            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            application.UseHangfireDashboard("/hangfire");
            application.Run();
        }
    }
}
