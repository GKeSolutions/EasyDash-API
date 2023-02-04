using Core.Interface;
using Core.Service;
using Hangfire;
using Hangfire.SqlServer;
using Notification;
using System.Text.Json.Serialization;

namespace EasyDash_API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                //options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            //builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
            //   .AddNegotiate();

            //builder.Services.AddAuthorization(options =>
            //{
            //    options.FallbackPolicy = options.DefaultPolicy;
            //});

            services.AddSingleton<IDashboardService, DashboardService>();
            services.AddSingleton<ILookupService, LookupService>();
            services.AddSingleton<IAnalyticsService, AnalyticsService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IScheduledNotificationService, ScheduledNotificationService>();
            services.AddSingleton<ISchedulerService, SchedulerService>();
            services.AddSingleton<IMissingTimeService, MissingTimeService>();
            services.AddSingleton<IJobService, JobService>();

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
        }

        public void Configure(IApplicationBuilder app, IBackgroundJobClient backgroundJobs, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseHangfireDashboard();
            //backgroundJobs.Enqueue(() => Console.WriteLine("Hello world from Hangfire!"));
        }
    }
}
