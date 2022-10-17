using Core.Interface;
using Core.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Core.Service
{
    public class ProcessService : IProcessService
    {
        private IConfiguration Configuration;

        public ProcessService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<object> GetProcesses(int orderBy)
        {
            var result =  await GetProcessesFromDb();
            switch (orderBy)
            {
                case 1:
                    return GetItemsGroupedByUser(result);
                case 2:
                    return GetItemsGroupedByProcess(result);
                default:
                    return GetItemsGroupedByProcess(result);
            }
        }

        private async Task<IEnumerable<ProcessResult>> GetProcessesFromDb()
        {
            var connection = new SqlConnection("Data Source=localhost;Initial Catalog=TE_3E_SANDBOX29;Integrated Security=true");
            connection.Open();
            return await connection.QueryAsync<ProcessResult>("GetProcesses");
        }

        private IEnumerable<User> GetItemsGroupedByUser(IEnumerable<ProcessResult> processes)
        {
            var grouped = processes.ToList()
                .GroupBy(r => r.UserId)
                .Select(g => new User
                {
                    UserName = g.First().UserName,
                    UserEmail = g.First().UserEmail,
                    UserId = g.First().UserId,
                    Processes = GetProcessesFromGroup(g)
                });
            return grouped;
        }

        private IEnumerable<User> GetItemsGroupedByProcess(IEnumerable<ProcessResult> processes)
        {
            var grouped = processes.ToList()
                .GroupBy(r => r.UserId)
                .Select(g => new User
                {
                    UserName = g.First().UserName,
                    UserEmail = g.First().UserEmail,
                    UserId = g.First().UserId,
                    Processes = GetProcessesFromGroup(g)
                });
            return grouped;
        }

        private List<ProcessItem> GetProcessesFromGroup(IGrouping<Guid, ProcessResult> g)
        {
            var result = new List<ProcessItem>();
            foreach (var x in g)
            {
                var pi = new ProcessItem
                {
                    LastUpdated = x.LastUpdated,
                    ProcessCode = x.ProcessCode,
                    ProcessDescription = x.ProcessDescription,
                    ProcessItemId = x.ProcessItemId
                };
                result.Add(pi);
            }
            return result;
        }
    }
}
