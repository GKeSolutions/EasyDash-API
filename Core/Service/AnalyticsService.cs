using Core.Interface;
using Core.Model.Analytics;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Core.Service
{
    public class AnalyticsService : IAnalyticsService
    {
        private IConfiguration Configuration;

        public AnalyticsService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<IEnumerable<UserAnalytic>> GetAnalyticUsers()
        {
            var analyticsData = await GetAnalyticsFromDb();
            return GroupByUser(analyticsData);
        }

        public async Task<IEnumerable<ProcessAnalytic>> GetAnalyticProcesses()
        {
            var analyticsData = await GetAnalyticsFromDb();
            return GroupByProcess(analyticsData);
        }

        public async Task<IEnumerable<User>> GetAnalyticUserList()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                StartDate= DateTime.Now.AddYears(-3),
                EndDate= DateTime.Now,
            });
            return await connection.QueryAsync<User>("ed.Analytics_UserList", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<Process>> GetAnalyticProcessList()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                StartDate = DateTime.Now.AddYears(-3),
                EndDate = DateTime.Now,
            });
            return await connection.QueryAsync<Process>("ed.Analytics_ProcessList", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        private async Task<IEnumerable<Analytics>> GetAnalyticsFromDb()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                StartDate = DateTime.Now.AddYears(-3),
                EndDate = DateTime.Now,
            });
            return await connection.QueryAsync<Analytics>("ed.Analytics", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        private IEnumerable<UserAnalytic> GroupByUser(IEnumerable<Analytics> analytics)
        {
            var grouped = analytics.ToList()
                .GroupBy(r => r.UserId)
                .Select(g => new UserAnalytic
                {
                    UserId = g.First().UserId,
                    UserName = g.First().UserName,
                    Processes = GetProcessesFromGroup(g)
                });
            return grouped;
        }

        private IEnumerable<ProcessAnalytic> GroupByProcess(IEnumerable<Analytics> analytics)
        {
            var grouped = analytics.ToList()
                .GroupBy(r => r.ProcessCode)
                .Select(g => new ProcessAnalytic
                {
                    ProcessCode = g.First().ProcessCode,
                    ProcessName = g.First().ProcessName,
                    Users = GetUsersFromGroup(g)
                }); ;
            return grouped;
        }

        private List<ProcessAnalytic> GetProcessesFromGroup(IGrouping<Guid, Analytics> g)
        {
            var result = new List<ProcessAnalytic>();
            foreach (var x in g)
            {
                var pa = new ProcessAnalytic
                {
                    ProcessCode = x.ProcessCode,
                    ProcessName = x.ProcessName,
                    AverageHours = Math.Round(TimeSpan.FromMinutes(x.AvgTimeSpentInMinutes).TotalHours, 2),
                    TotalHours = Math.Round(TimeSpan.FromMinutes(x.TotalTimeSpentInMinutes).TotalHours, 2),
                };
                result.Add(pa);
            }
            return result;
        }

        private List<UserAnalytic> GetUsersFromGroup(IGrouping<string, Analytics> g)
        {
            var result = new List<UserAnalytic>();
            foreach (var x in g)
            {
                var user = new UserAnalytic
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    AverageHours = Math.Round(TimeSpan.FromMinutes(x.AvgTimeSpentInMinutes).TotalHours, 2),
                    TotalHours = Math.Round(TimeSpan.FromMinutes(x.TotalTimeSpentInMinutes).TotalHours, 2),
                };
                result.Add(user);
            }
            return result.OrderByDescending(x => x.AverageHours).ToList();
        }
    }
}
