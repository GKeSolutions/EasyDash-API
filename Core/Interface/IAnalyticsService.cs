using Core.Model.Analytics;

namespace Core.Interface
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<UserAnalytic>> GetAnalyticUsers(DateTime startDate, DateTime endDate);
        Task<IEnumerable<ProcessAnalytic>> GetAnalyticProcesses(DateTime startDate, DateTime endDate);
        Task<IEnumerable<User>> GetAnalyticUserList();
        Task<IEnumerable<Process>> GetAnalyticProcessList();
    }
}
