using Core.Model.Notification;

namespace Core.Interface;

public interface ISchedulerService
{
    Task<IEnumerable<Scheduler>> GetScheduler();
    Task<Scheduler> CreateScheduler(Scheduler scheduler);
    Task<Scheduler> UpdateScheduler(Scheduler scheduler);
    Task<int> DeleteScheduler(int scheduler);
}

