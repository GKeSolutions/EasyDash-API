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
        public async Task<IEnumerable<UserAnalytic>> GetAnalytics()
        {
            var analyticsData = await GetAnalyticsFromDb();
            return GroupByUser(analyticsData);
        }

        private async Task<IEnumerable<Analytics>> GetAnalyticsFromDb()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Analytics>("Analytics");
        }

        private IEnumerable<UserAnalytic> GroupByUser(IEnumerable<Analytics> analytics)
        {
            var test = analytics.ToList().GroupBy(r => r.UserId);
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

        private List<ProcessAnalytic> GetProcessesFromGroup(IGrouping<Guid, Analytics> g)
        {
            var result = new List<ProcessAnalytic>();
            foreach (var x in g)
            {
                var pa = new ProcessAnalytic
                {
                    ProcessCode = x.ProcessCode,
                    ProcessName = x.ProcessName,
                    AverageHours = x.AvgTimeSpentInMinutes
                };
                result.Add(pa);
            }
            return result;
        }
    }
}
