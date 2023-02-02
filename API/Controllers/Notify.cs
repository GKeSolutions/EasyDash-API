using Core.Interface;
using Core.Model.Notification;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Notify : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private readonly INotificationService NotificationService;

        public Notify(IConfiguration configuration, INotificationService notificationService)
        {
            Configuration = configuration;
            NotificationService = notificationService;
        }
        [HttpPost]
        public async Task<bool> Send([FromBody] EmailNotification emailNotification)
        {
            return await NotificationService.SendEmailNotification(emailNotification);
        }
    }
}
