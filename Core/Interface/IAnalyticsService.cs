using Core.Model.Analytics;

namespace Core.Interface
{
    public interface IAnalyticsService
    {
        Task<IEnumerable<UserAnalytic>> GetAnalytics();
    }
}
