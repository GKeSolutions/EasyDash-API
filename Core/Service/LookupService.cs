using Core.Interface;
using Core.Model.Dashboard.Role;
using Core.Model.Dashboard.User;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Core.Service
{
    public class LookupService : ILookupService
    {
        private IConfiguration Configuration;
        private readonly ILogger<LookupService> Logger;
        private IHttpContextAccessor HttpContextAccessor;
        private string UserName;

        public LookupService(IConfiguration configuration, ILogger<LookupService> logger, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            Logger = logger;
            UserName = HttpContextAccessor.HttpContext.User.Identity.Name;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            Logger.LogInformation($"{UserName} - {nameof(LookupService)} - {nameof(GetRoles)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Role>("ed.GetRoles");
        }

        public async Task<IEnumerable<UsersRole>> GetUsersRoles()
        {
            Logger.LogInformation($"{UserName} - {nameof(LookupService)} - {nameof(GetUsersRoles)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<UsersRole>("ed.GetAllUsersAndRoles");
        }

        public async Task<Guid> GetUserIdByNetworkAlias(string networkAlias)
        {
            Logger.LogInformation($"{UserName} - {nameof(LookupService)} - {nameof(GetUsersRoles)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                NetworkAlias=networkAlias
            });
            return await connection.QueryFirstOrDefaultAsync<Guid>("ed.GetUserIdByNetworkAlias", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public bool GetIsActive3EUser(string networkAlias)
        {
            Logger.LogInformation($"{UserName} - {nameof(LookupService)} - {nameof(GetIsActive3EUser)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                NetworkAlias = networkAlias
            });
            return connection.QueryFirstOrDefault<bool>("ed.GetIsActive3EUser", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public IEnumerable<Role> GetRolesPerNetworkAlias(string networkAlias)
        {
            Logger.LogInformation($"{UserName} - {nameof(LookupService)} - {nameof(GetRolesPerNetworkAlias)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                NetworkAlias = networkAlias
            });
            return connection.Query<Role>("ed.GetRolesPerNetworkAlias", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<string> GetUserNameByNetworkAlias()
        {
            Logger.LogInformation($"{UserName} - {nameof(LookupService)} - {nameof(GetUserNameByNetworkAlias)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                NetworkAlias = UserName
            });
            return await connection.QueryFirstOrDefaultAsync<string>("ed.GetUserNameByNetworkAlias", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
