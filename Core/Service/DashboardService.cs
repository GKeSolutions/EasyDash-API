using Core.Interface;
using Core.Model.Dashboard.Process;
using Core.Model.Dashboard.Role;
using Core.Model.Dashboard.User;
using Core.Model.Notification;
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

        public async Task<IEnumerable<DashUser>> GetUsers()
        {
            var processes = await GetProcessesFromDb();
            var grouped = GroupByUser(processes);
            var distinctUsers = grouped.Select(x => x.UserId).Distinct();
            var userRoles = await GetUsersRoles(distinctUsers);
            return MapUserToRoles(grouped.ToList(), userRoles);
        }

        public async Task<IEnumerable<DashUser>> GetUsersByProcess(string processCode)
        {
            var processes = await GetUsersByProcessFromDb(processCode);
            var grouped = GroupByUser(processes);
            var distinctUsers = grouped.Select(x => x.UserId).Distinct();
            var userRoles = await GetUsersRoles(distinctUsers);
            return MapUserToRoles(grouped.ToList(), userRoles);
        }

        public async Task<IEnumerable<DashProcess>> GetProcesses()
        {
            var processes = await GetProcessesFromDb();
            var grouped = GroupByProcess(processes);
            var distinctUsers = processes.Select(x => x.UserId).Distinct();
            var userRoles = await GetUsersRoles(distinctUsers);
            var processWithUsersAndRoles = new List<DashProcess>();
            foreach (var p in grouped)
            {
                p.Users = MapUserToRoles(p.Users, userRoles).ToList();
                processWithUsersAndRoles.Add(p);
            }
            return processWithUsersAndRoles;
        }

        public async Task<IEnumerable<DashProcess>> GetProcessesByUser(Guid userId)
        {
            var processes = await GetProcessesByUserFromDb(userId);
            var grouped = GroupByProcess(processes);
            var distinctUsers = processes.Select(x => x.UserId).Distinct();
            var userRoles = await GetUsersRoles(distinctUsers);
            var processWithUsersAndRoles = new List<DashProcess>();
            foreach (var p in grouped)
            {
                p.Users = MapUserToRoles(p.Users, userRoles).ToList();
                processWithUsersAndRoles.Add(p);
            }
            return processWithUsersAndRoles;
        }

        public async Task<IEnumerable<ProcessResult>> GetOpenProcessesPerTemplate(int templateId)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                TemplateId=templateId,
            });
            return await connection.QueryAsync<ProcessResult>("ed.GetOpenProcessesPerTemplate", param: dparam, commandType: CommandType.StoredProcedure);
        }

        public async Task<ProcessResult> GetProcessInfoByProcId(string processId)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                ProcessId=processId,
            });
            return await connection.QueryFirstOrDefaultAsync<ProcessResult>("ed.GetProcessInfoByProcId", param: dparam, commandType: CommandType.StoredProcedure);
        }

        private async Task<IEnumerable<ProcessResult>> GetProcessesFromDb()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<ProcessResult>("ed.GetProcesses");
        }

        private async Task<IEnumerable<ProcessResult>> GetProcessesByUserFromDb(Guid userId)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                userId
            });
            return await connection.QueryAsync<ProcessResult>("ed.GetOpenProcessesByUser", param: dparam, commandType: CommandType.StoredProcedure);
        }

        private async Task<IEnumerable<ProcessResult>> GetUsersByProcessFromDb(string processCode)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                process=processCode
            });
            return await connection.QueryAsync<ProcessResult>("ed.GetUsersByProcess", param: dparam, commandType: CommandType.StoredProcedure);
        }

        private IEnumerable<DashUser> GroupByUser(IEnumerable<ProcessResult> processes)
        {
            var grouped = processes.ToList()
                .GroupBy(r => r.UserId)
                .Select(g => new DashUser
                {
                    UserName = g.First().UserName,
                    UserEmail = g.First().UserEmail,
                    UserId = g.First().UserId,
                    ProcessCaption = g.First().ProcessCaption,
                    Processes = GetProcessesFromGroup(g)
                });
            return grouped;
        }

        private IEnumerable<DashProcess> GroupByProcess(IEnumerable<ProcessResult> processes)
        {
            var grouped = processes.ToList()
                .GroupBy(r => r.ProcessCode)
                .Select(g => new DashProcess
                {
                    ProcessCode = g.First().ProcessCode,
                    ProcessDescription = g.First().ProcessDescription,
                    ProcessCaption = g.First().ProcessCaption,
                    Users = GetUsersFromGroup(g),
                    UserName = g.First().UserName
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
                    ProcessItemId = x.ProcessItemId,
                    ProcessCaption = x.ProcessCaption,
                };
                result.Add(pi);
            }
            return result;
        }

        private List<DashUser> GetUsersFromGroup(IGrouping<string, ProcessResult> g)
        {
            var result = new List<DashUser>();
            foreach (var x in g)
            {
                var user = new DashUser
                {
                    UserId = x.UserId,
                    UserName = x.UserName,
                    UserEmail = x.UserEmail,
                    LastUpdated = x.LastUpdated
                    
                };
                result.Add(user);
            }
            return result;
        }

        private async Task<IEnumerable<UserRole>> GetUsersRoles(IEnumerable<Guid> users)
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
                users = usersTbl.AsTableValuedParameter("ed.IdListGui"),
            });

            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<UserRole>(sql: "ed.GetUsersRoles", param: dparam, commandType: CommandType.StoredProcedure);
        }

        private IEnumerable<DashUser> MapUserToRoles(List<DashUser> grouped, IEnumerable<UserRole> userRoles)
        {
            var updatedUsersWithRoles = new List<DashUser>();
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
