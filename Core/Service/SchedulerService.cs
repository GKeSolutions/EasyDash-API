using Core.Interface;
using Core.Model.Notification;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Core.Service
{
    public class SchedulerService : ISchedulerService
    {
        private readonly IJobService JobService;
        private IConfiguration Configuration;
        private readonly ILogger<SchedulerService> Logger;
        private IHttpContextAccessor HttpContextAccessor;
        private string UserName;

        public SchedulerService(IJobService jobService, IConfiguration configuration, ILogger<SchedulerService> logger, IHttpContextAccessor httpContextAccessor)
        {
            JobService = jobService;
            Configuration = configuration;
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            UserName = HttpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }

        #region Scheduler
        public async Task<IEnumerable<Scheduler>> GetScheduler()
        {
            Logger.LogInformation($"{UserName} - {nameof(SchedulerService)} - {nameof(GetScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Scheduler>("ed.GeScheduler");
        }

        public async Task<Scheduler> CreateScheduler(Scheduler scheduler)
        {
            Logger.LogInformation($"{UserName} - {nameof(SchedulerService)} - {nameof(CreateScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                CronExpression = scheduler.CronExpression,
                Description = scheduler.Description,
            });
            return await connection.QueryFirstOrDefaultAsync<Scheduler>("ed.CreateScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Scheduler> UpdateScheduler(Scheduler scheduler)
        {
            Logger.LogInformation($"{UserName} - {nameof(SchedulerService)} - {nameof(UpdateScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {   Id=scheduler.Id,
                CronExpression = scheduler.CronExpression,
                Description = scheduler.Description
            });
            var result = await connection.QueryFirstOrDefaultAsync<Scheduler>("ed.UpdateScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);

            var linkedScheduledNotifications = await GetLinkedScheduledNotifications(scheduler.Id);
            if (linkedScheduledNotifications.Count() > 0)
            {
                foreach (var scheduledNotification in linkedScheduledNotifications)
                {
                    JobService.DeleteJob(scheduledNotification.Id.ToString());
                    if (scheduledNotification.IsActive) JobService.AddJob(scheduledNotification);
                }
            }

            return result;
        }

        public async Task<int> DeleteScheduler(int scheduler)
        {
            Logger.LogInformation($"{UserName} - {nameof(SchedulerService)} - {nameof(DeleteScheduler)} {scheduler}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = scheduler
            });
            return await connection.ExecuteAsync("ed.DeleteScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        private async Task<IEnumerable<ScheduledNotification>> GetLinkedScheduledNotifications(int schedulerId)
        {
            Logger.LogInformation($"{UserName} - {nameof(SchedulerService)} - {nameof(UpdateScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                schedulerId
            });
            return await connection.QueryAsync<ScheduledNotification>("ed.IsSchedulerLinked", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }
        #endregion
    }
}
