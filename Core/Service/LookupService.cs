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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Role>("ed.GetRoles");
        }

        public async Task<IEnumerable<UsersRole>> GetUsersRoles()
        {
            Logger.LogInformation($"{nameof(LookupService)} - {nameof(GetUsersRoles)}");
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<UsersRole>("ed.GetAllUsersAndRoles");
        }
    }
}
