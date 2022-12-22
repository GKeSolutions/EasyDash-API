using EasyDash_API;
public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
//var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
//});
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
////builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
////   .AddNegotiate();

////builder.Services.AddAuthorization(options =>
////{
////    options.FallbackPolicy = options.DefaultPolicy;
////});

//builder.Services.AddSingleton<IDashboardService, DashboardService>();
//builder.Services.AddSingleton<ILookupService, LookupService>();
//builder.Services.AddSingleton<IAnalyticsService, AnalyticsService>();
//builder.Services.AddSingleton<INotificationService, NotificationService>();
//builder.Services.AddSingleton<IMissingTimeService, MissingTimeService>();


//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();
