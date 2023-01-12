using Core.Interface;
using Core.Service;
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
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
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
            services.AddSingleton<IMissingTimeService, MissingTimeService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
