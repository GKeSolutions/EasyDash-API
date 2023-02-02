using Core.Interface;
using Core.Model.Notification;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Core.Service
{
    public class SchedulerService : ISchedulerService
    {
        private IConfiguration Configuration;
        public SchedulerService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        #region Scheduler
        public async Task<IEnumerable<Scheduler>> GetScheduler()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Scheduler>("ed.GeScheduler");
        }

        public async Task<Scheduler> CreateScheduler(Scheduler scheduler)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
