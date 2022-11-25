using Core.Interface;
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
        public List<Type> Type()
        {
            return NotificationService.Type();
        }
    }
}
