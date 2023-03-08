using API;
using Core.Interface;
using Core.Service;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Notification;
using System.Text.Json.Serialization;

namespace EasyDash_API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            //    .AddNegotiate();

            //services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = options.DefaultPolicy;
            //});
            services.AddCors(options =>
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
            services.AddSingleton(emailConfig);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            services.AddTransient<IDashboardService, DashboardService>();
            services.AddTransient<ILookupService, LookupService>();
            services.AddTransient<IAnalyticsService, AnalyticsService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IScheduledNotificationService, ScheduledNotificationService>();
            services.AddTransient<ISchedulerService, SchedulerService>();
            services.AddTransient<IMissingTimeService, MissingTimeService>();
            services.AddTransient<IJobService, JobService>();
            services.AddTransient<IReassignService, ReassignService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAdd(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactory>());
            services.TryAdd(ServiceDescriptor.Singleton(typeof(ILogger<ExceptionMiddleware>), typeof(Logger<ExceptionMiddleware>)));

            services.AddHangfire(configuration => configuration
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
            services.AddHangfireServer();
            services.AddHttpLogging(options =>
            {
                options.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseHttpLogging();
            var path = Directory.GetCurrentDirectory();
            loggerFactory.AddFile($"{path}\\Logs\\Log.txt");
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(MyAllowSpecificOrigins);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHangfireDashboard("/hangfire");
        }
    }
}
