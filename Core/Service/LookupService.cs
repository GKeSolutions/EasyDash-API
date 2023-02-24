using Core.Interface;
using Core.Model.Dashboard.Role;
using Core.Model.Dashboard.User;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Core.Service
{
    public class LookupService : ILookupService
    {
        private IConfiguration Configuration;

        public LookupService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Role>("ed.GetRoles");
        }

        //GetAllUsersAndRoles
        public async Task<IEnumerable<UsersRole>> GetUsersRoles()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<UsersRole>("ed.GetAllUsersAndRoles");
        }
    }
}
