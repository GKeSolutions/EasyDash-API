using Core.Model.Analytics;

namespace Core.Interface
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<UserAnalytic>> GetAnalyticUsers();
        Task<IEnumerable<ProcessAnalytic>> GetAnalyticProcesses();
        Task<IEnumerable<User>> GetAnalyticUserList();
        Task<IEnumerable<Process>> GetAnalyticProcessList();
    }
}
