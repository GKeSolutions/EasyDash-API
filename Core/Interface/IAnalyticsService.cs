using Core.Model.Analytics;

namespace Core.Interface
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<UserAnalytic>> GetAnalyticUsers();
        Task<IEnumerable<ProcessAnalytic>> GetAnalyticProcesses();
    }
}
