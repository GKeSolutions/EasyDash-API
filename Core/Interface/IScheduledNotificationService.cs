using Core.Model.Notification;

namespace Core.Interface;

public interface IScheduledNotificationService
{
    Task<IEnumerable<ScheduledNotification>> GetScheduledNotification();
    Task<ScheduledNotification> CreateScheduledNotification(ScheduledNotification scheduler);
    Task<ScheduledNotification> UpdateScheduledNotification(ScheduledNotification scheduler);
    Task<int> DeleteScheduledNotification(int scheduler);
}

