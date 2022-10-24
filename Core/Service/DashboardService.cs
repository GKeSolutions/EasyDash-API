using Core.Interface;
using Core.Model;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Core.Service
{
    public class DashboardService : IDashboardService
    {
        private IConfiguration Configuration;

        public DashboardService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var processes = await GetProcessesFromDb();
            var grouped = GroupByUser(processes);
            var distinctUsers = grouped.Select(x => x.UserId).Distinct();
            var userRoles = await GetUserRoles(distinctUsers);
            return MapUserToRoles(grouped.ToList(), userRoles);
        }

        public async Task<IEnumerable<Process>> GetProcesses()
        {
            var processes = await GetProcessesFromDb();
            var grouped = GroupByProcess(processes);
            var distinctUsers = processes.Select(x => x.UserId).Distinct();
            var userRoles = await GetUserRoles(distinctUsers);
            var processWithUsersAndRoles = new List<Process>();
            foreach (var p in grouped)
            {
                p.Users = MapUserToRoles(p.Users, userRoles).ToList();
                processWithUsersAndRoles.Add(p);
            }
            return processWithUsersAndRoles;
        }

        private async Task<IEnumerable<ProcessResult>> GetProcessesFromDb()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<ProcessResult>("GetProcesses");
        }

        private IEnumerable<User> GroupByUser(IEnumerable<ProcessResult> processes)
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

        private IEnumerable<Process> GroupByProcess(IEnumerable<ProcessResult> processes)
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
                };
                result.Add(user);
            }
            return result;
        }

        private async Task<IEnumerable<UserRole>> GetUserRoles(IEnumerable<Guid> users)
        {
            var usersTbl = new DataTable();
            usersTbl.Columns.Add("Id", typeof(Guid));
            if (users != null)
            {
                foreach (var id in users)
                    usersTbl.Rows.Add(id);
            }
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                users = usersTbl.AsTableValuedParameter("IdListGui"),
            });

            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<UserRole>(sql: "GetUserRoles", param: dparam, commandType: CommandType.StoredProcedure);
        }

        private IEnumerable<User> MapUserToRoles(List<User> grouped, IEnumerable<UserRole> userRoles)
        {
            var updatedUsersWithRoles = new List<User>();
            foreach (var u in grouped)
            {
                foreach (var g in userRoles)
                {
                    if(u.UserId == g.UserId)
                    {
                        u.Roles.Add(new Role { RoleId = g.RoleId, RoleName = g.RoleName });
                    }
                }
                updatedUsersWithRoles.Add(u);
            }
            return updatedUsersWithRoles;
        }
    }
}
