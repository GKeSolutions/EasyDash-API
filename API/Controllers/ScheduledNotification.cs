using Core.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ScheduledNotificationModel = Core.Model.Notification.ScheduledNotification;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ScheduledNotification : ControllerBase
    {
        private IScheduledNotificationService ScheduledNotificationService { get; set; }
        public ScheduledNotification(IScheduledNotificationService sheduledNotificationService)
        {
            ScheduledNotificationService = sheduledNotificationService;
        }

        #region ScheduledNotification
        [HttpGet]
        public async Task<IEnumerable<ScheduledNotificationModel>> GetScheduledNotification()
        {
            return await ScheduledNotificationService.GetScheduledNotification();
        }

        [HttpPost]
        public async Task<ScheduledNotificationModel> CreateScheduledNotification([FromBody] ScheduledNotificationModel scheduledNotification)
        {
            return await ScheduledNotificationService.CreateScheduledNotification(scheduledNotification);
        }

        [HttpPut]
        public async Task<ScheduledNotificationModel> UpdateScheduledNotification([FromBody] ScheduledNotificationModel scheduledNotification)
        {
            return await ScheduledNotificationService.UpdateScheduledNotification(scheduledNotification);
        }

        [HttpDelete]
        public async Task<int> DeleteScheduledNotification(int scheduledNotification)
        {
            return await ScheduledNotificationService.DeleteScheduledNotification(scheduledNotification);
        }
        #endregion
    }
}
