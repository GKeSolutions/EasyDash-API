using Core.Interface;
using Core.Model.Notification;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Core.Service
{
    public class ScheduledNotificationService : IScheduledNotificationService
    {
        private IConfiguration Configuration;
        private IJobService JobService;

        public ScheduledNotificationService(IConfiguration configuration, IJobService jobService)
        {
            Configuration = configuration;
            JobService = jobService;
        }

        #region ScheduledNotification
        public async Task<IEnumerable<ScheduledNotification>> GetScheduledNotification()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<ScheduledNotification>("ed.GeNotificationScheduler");
        }

        public async Task<ScheduledNotification> CreateScheduledNotification(ScheduledNotification scheduledNotification)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                IsActive = scheduledNotification.IsActive,
                NotificationTemplate = scheduledNotification.NotificationTemplate,
                Scheduler = scheduledNotification.Scheduler,
                NotifyAfterDays = scheduledNotification.NotifyAfterDays,
                ReassignTo = scheduledNotification.ReassignTo,
                CcContact = scheduledNotification.CcContact
            });
            var result = await connection.QueryFirstOrDefaultAsync<ScheduledNotification>("ed.CreateScheduledNotification", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            JobService.AddJob(scheduledNotification);
            return result;
        }

        public async Task<ScheduledNotification> UpdateScheduledNotification(ScheduledNotification scheduledNotification)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = scheduledNotification.Id,
                IsActive = scheduledNotification.IsActive,
                NotificationTemplate = scheduledNotification.NotificationTemplate,
                Priority = scheduledNotification.Scheduler,
                Role = scheduledNotification.NotifyAfterDays,
                Process = scheduledNotification.ReassignTo,
                TemplateSubject = scheduledNotification.CcContact
            });
            var result = await connection.QueryFirstOrDefaultAsync<ScheduledNotification>("ed.UpdateScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            JobService.DeleteJob(scheduledNotification.Id.ToString());
            JobService.AddJob(scheduledNotification);
            return result;
        }

        public async Task<int> DeleteScheduledNotification(int scheduledNotification)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = scheduledNotification
            });
            return await connection.ExecuteAsync("ed.DeleteScheduledNotification", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }
        #endregion
    }
}
