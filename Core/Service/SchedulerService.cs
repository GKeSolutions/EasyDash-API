using Core.Interface;
using Core.Model.Notification;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Core.Service
{
    public class SchedulerService : ISchedulerService
    {
        private IConfiguration Configuration;
        private readonly ILogger<SchedulerService> Logger;

        public SchedulerService(IConfiguration configuration, ILogger<SchedulerService> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        #region Scheduler
        public async Task<IEnumerable<Scheduler>> GetScheduler()
        {
            Logger.LogInformation($"{nameof(SchedulerService)} - {nameof(GetScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Scheduler>("ed.GeScheduler");
        }

        public async Task<Scheduler> CreateScheduler(Scheduler scheduler)
        {
            Logger.LogInformation($"{nameof(SchedulerService)} - {nameof(CreateScheduler)}");
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
            Logger.LogInformation($"{nameof(SchedulerService)} - {nameof(UpdateScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {   Id=scheduler.Id,
                CronExpression = scheduler.CronExpression,
                Description = scheduler.Description
            });
            return await connection.QueryFirstOrDefaultAsync<Scheduler>("ed.UpdateScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> DeleteScheduler(int scheduler)
        {
            Logger.LogInformation($"{nameof(SchedulerService)} - {nameof(DeleteScheduler)} {scheduler}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = scheduler
            });
            return await connection.ExecuteAsync("ed.DeleteScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }
        #endregion
    }
}
