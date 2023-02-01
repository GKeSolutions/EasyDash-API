using Core.Interface;
using Core.Model.Notification;
using Microsoft.AspNetCore.Mvc;
using Type = Core.Model.Notification.Type;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Notification : ControllerBase
    {
        private INotificationService NotificationService { get; set; }

        public Notification(INotificationService notificationService)
        {
            NotificationService = notificationService;
        }

        [HttpGet]
        public async Task<IEnumerable<Type>> Type()
        {
            return await NotificationService.GetType();
        }

        [HttpGet]
        public async Task<IEnumerable<Template>> NotificationTemplate()
        {
            return await NotificationService.GetTemplate();
        }

        [HttpPost]
        public async Task<Template> CreateTemplate([FromBody] Template template)
        {
            return await NotificationService.CreateTemplate(template);
        }

        [HttpPut]
        public async Task<Template> UpdateTemplate([FromBody] Template template)
        {
            return await NotificationService.UpdateTemplate(template);
        }

        [HttpDelete]
        public async Task<int> DeleteTemplate(int template)
        {
            return await NotificationService.DeleteTemplate(template);
        }

        #region ScheduledNotification
        [HttpGet]
        public async Task<IEnumerable<ScheduledNotification>> GetScheduledNotification()
        {
            return await NotificationService.GetScheduledNotification();
        }

        [HttpPost]
        public async Task<ScheduledNotification> CreateScheduledNotification([FromBody] ScheduledNotification scheduledNotification)
        {
            return await NotificationService.CreateScheduledNotification(scheduledNotification);
        }

        [HttpPut]
        public async Task<ScheduledNotification> UpdateScheduledNotification([FromBody] ScheduledNotification scheduledNotification)
        {
            return await NotificationService.UpdateScheduledNotification(scheduledNotification);
        }

        [HttpDelete]
        public async Task<int> DeleteScheduledNotification(int scheduledNotification)
        {
            return await NotificationService.DeleteScheduledNotification(scheduledNotification);
        }
        #endregion

        #region Scheduler
        [HttpGet]
        public async Task<IEnumerable<Scheduler>> GetScheduler()
        {
            return await NotificationService.GetScheduler();
        }

        [HttpPost]
        public async Task<Scheduler> CreateScheduler([FromBody] Scheduler scheduler)
        {
            return await NotificationService.CreateScheduler(scheduler);
        }

        [HttpPut]
        public async Task<Scheduler> UpdateScheduler([FromBody] Scheduler scheduler)
        {
            return await NotificationService.UpdateScheduler(scheduler);
        }

        [HttpDelete]
        public async Task<int> DeleteScheduler(int scheduler)
        {
            return await NotificationService.DeleteScheduler(scheduler);
        }
        #endregion
    }
}
