using Core.Interface;
using Core.Model.MissingTime;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Core.Service
{
    public class MissingTimeService : IMissingTimeService
    {
        private IConfiguration Configuration;
        public MissingTimeService(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<IEnumerable<Time>> GetMissingTime()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                StartDate = DateTime.Now.AddYears(-10),
                EndDate = DateTime.Now,
            });
            return await connection.QueryAsync<Time>("ed.GetMissingTime", param: dparam, commandType: System.Data.CommandType.StoredProcedure);

        }
    }
}
