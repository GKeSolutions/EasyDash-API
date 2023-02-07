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

        public async Task<IEnumerable<MissingTime>> GetMissingTime(DateTime startDate, DateTime endDate)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                StartDate = startDate,
                EndDate = endDate,
            });
            var result = await connection.QueryAsync<Time>("ed.GetMissingTime", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            return GroupByUser(result);
        }

        public async Task<Time> GetTimePerUserPerWeek(Guid userId, DateTime startDate, DateTime endDate)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
            });
            var result = await connection.QueryFirstAsync<Time>("ed.GetTimePerUserPerWeek", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Time>> GetUsersTimePerWeek(DateTime startDate, DateTime endDate)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                StartDate = startDate,
                EndDate = endDate,
            });
            var result = await connection.QueryAsync<Time>("ed.GetUsersWithMissingTimePerWeek", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Time>> GetWeeksTimePerUser(Guid userId, DateTime startDate, DateTime endDate)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                UserId = userId,
                StartDate = startDate,
                EndDate = endDate,
            });
            var result = await connection.QueryAsync<Time>("ed.GetWeeksTimePerUser", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<IEnumerable<Time>> GetMissingTimeUsersPerTemplate(int templateId)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                TemplateId= templateId,
            });
            var result = await connection.QueryAsync<Time>("ed.GetMissingTimeUsersPerTemplate", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        public async Task<string> GetCcContactEmailAddress(Guid ccContact)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                ccContact = ccContact,
            });
            var result = await connection.QueryFirstOrDefaultAsync<string>("ed.GetCcContactEmailAddress", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
            return result;
        }

        private IEnumerable<MissingTime> GroupByUser(IEnumerable<Time> times)
        {
            return times.ToList()
                .GroupBy(r => r.UserId)
                .Select(g => new MissingTime
                {
                    Name = g.First().UserName,
                    Expected = g.First().WeeklyHoursRequired,
                    EmailAddress = g.First().EmailAddress,
                    UserId = g.First().UserId,
                    Days = BuildDays(g)
                });
        }

        private List<Day> BuildDays(IGrouping<Guid, Time> g)
        {
            var result = new List<Day>();
            foreach (var x in g)
            {
                var day = new Day
                {
                    Date = x.WorkDate,
                    Logged = x.WorkHrs
                };
                result.Add(day);
            }
            return result;
        }
    }
}
