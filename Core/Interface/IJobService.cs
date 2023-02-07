using Core.Model.Notification;

namespace Core.Interface
{
    public interface IJobService
    {
        bool AddJob(ScheduledNotification scheduledNotification);
        bool DeleteJob(string jobId);
    }
}
