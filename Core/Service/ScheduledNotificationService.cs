using Core.Interface;
using Core.Model.Notification;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Core.Service
{
    public class ScheduledNotificationService : IScheduledNotificationService
    {
        private IConfiguration Configuration;
        private IJobService JobService;
        private readonly ILogger<ScheduledNotificationService> Logger;
        private IHttpContextAccessor HttpContextAccessor;
        private string UserName;

        public ScheduledNotificationService(IConfiguration configuration, IJobService jobService, ILogger<ScheduledNotificationService> logger, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            JobService = jobService;
            Logger = logger;
            HttpContextAccessor= httpContextAccessor;
            UserName = HttpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }

        #region ScheduledNotification
        public async Task<IEnumerable<ScheduledNotification>> GetScheduledNotification()
        {
            Logger.LogInformation($"{UserName} - {nameof(ScheduledNotificationService)} - {nameof(GetScheduledNotification)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<ScheduledNotification>("ed.GetScheduledNotification");
        }

        public async Task<ScheduledNotification> CreateScheduledNotification(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{UserName} - {nameof(ScheduledNotificationService)} - {nameof(CreateScheduledNotification)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                IsActive = scheduledNotification.IsActive,
                NotificationTemplate = scheduledNotification.NotificationTemplate,
                Scheduler = scheduledNotification.Scheduler,
                NotifyAfterDays = scheduledNotification.NotifyAfterDays,
                ReassignTo = scheduledNotification.ReassignTo,
                CcContact = scheduledNotification.CcContact,
                IsCancelProcess = scheduledNotification.IsCancelProcess
            });
            var result = await connection.QueryFirstOrDefaultAsync<ScheduledNotification>("ed.CreateScheduledNotification", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            JobService.AddJob(result);
            return result;
        }

        public async Task<ScheduledNotification> UpdateScheduledNotification(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{UserName} - {nameof(ScheduledNotificationService)} - {nameof(UpdateScheduledNotification)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                scheduledNotification.Id,
                scheduledNotification.IsActive,
                scheduledNotification.NotificationTemplate,
                scheduledNotification.Scheduler,
                scheduledNotification.NotifyAfterDays,
                scheduledNotification.ReassignTo,
                scheduledNotification.CcContact,
                scheduledNotification.IsCancelProcess
            });
            var result = await connection.QueryFirstOrDefaultAsync<ScheduledNotification>("ed.UpdateScheduledNotification", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            JobService.DeleteJob(scheduledNotification.Id.ToString());
            JobService.AddJob(result);
            return result;
        }

        public async Task<int> DeleteScheduledNotification(int scheduledNotification)
        {
            Logger.LogInformation($"{UserName} - {nameof(ScheduledNotificationService)} - {nameof(DeleteScheduledNotification)} {scheduledNotification}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = scheduledNotification
            });
            var result = await connection.ExecuteAsync("ed.DeleteScheduledNotification", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            JobService.DeleteJob(scheduledNotification.ToString());
            return result;
        }
        #endregion
    }
}
