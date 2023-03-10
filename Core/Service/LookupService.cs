using Core.Interface;
using Core.Model.Dashboard.Role;
using Core.Model.Dashboard.User;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;

namespace Core.Service
{
    public class LookupService : ILookupService
    {
        private IConfiguration Configuration;
        private readonly ILogger<LookupService> Logger;

        public LookupService(IConfiguration configuration, ILogger<LookupService> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            Logger.LogInformation($"{nameof(LookupService)} - {nameof(GetRoles)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Role>("ed.GetRoles");
        }

        public async Task<IEnumerable<UsersRole>> GetUsersRoles()
        {
            Logger.LogInformation($"{nameof(LookupService)} - {nameof(GetUsersRoles)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<UsersRole>("ed.GetAllUsersAndRoles");
        }

        public async Task<Guid> GetUserIdByNetworkAlias(string networkAlias)
        {
            Logger.LogInformation($"{nameof(LookupService)} - {nameof(GetUsersRoles)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                NetworkAlias=networkAlias
            });
            return await connection.QueryFirstOrDefaultAsync<Guid>("ed.GetUserIdByNetworkAlias", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
