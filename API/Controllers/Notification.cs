using Core.Interface;
using Core.Model.Notification;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Type = Core.Model.Notification.Type;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class Notification : BaseController
    {
        private INotificationService NotificationService { get; set; }

        public Notification(INotificationService notificationService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
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

        [HttpPost]
        public async Task<IEnumerable<MessageHistory>> GetNotificationHistory(NotificationHistoryFilter filter)
        {
            return await NotificationService.GetNotificationHistory(filter);
        }
    }
}
