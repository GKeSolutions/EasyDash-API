using Core.Enum;
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
            var processes = await GetProcessesFromDb();
            switch (orderBy)
            {
                case (int)ProcessGroupBy.GroupByUser:
                    return GetProcessesGroupedByUser(processes);
                default:
                    return GetProcessesGroupedByProcess(processes);
            }
        }

        private async Task<IEnumerable<ProcessResult>> GetProcessesFromDb()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<ProcessResult>("GetProcesses");
        }

        private IEnumerable<User> GetProcessesGroupedByUser(IEnumerable<ProcessResult> processes)
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

        private IEnumerable<Process> GetProcessesGroupedByProcess(IEnumerable<ProcessResult> processes)
        {
            var grouped = processes.ToList()
                .GroupBy(r => r.ProcessCode)
                .Select(g => new Process
                {
                    ProcessCode = g.First().ProcessCode,
                    ProcessDescription = g.First().ProcessDescription,
                    Users = GetUsersFromGroup(g)
                }); ;
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

        private List<User> GetUsersFromGroup(IGrouping<string, ProcessResult> g)
        {
            var result = new List<User>();
            foreach (var x in g)
            {
                var user = new User
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    UserEmail = x.UserEmail,
                    //Processes = GetProcessesFromGroup2(g)
                };
                result.Add(user);
            }
            return result;
        }

        private List<ProcessItem> GetProcessesFromGroup2(IGrouping<string, ProcessResult> g)
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
