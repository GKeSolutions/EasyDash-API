using Core.Interface;
using Core.Model.Notification;
using Microsoft.AspNetCore.Mvc;
using Notification;
using System.Text.Json;

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
        public bool Send([FromBody] JsonElement payload)
        {
            var message = new Message(new string[] { payload.GetProperty("To").ToString() }, new string[] { payload.GetProperty("To").ToString() }, payload.GetProperty("Subject").ToString(), payload.GetProperty("Content").ToString());
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            var emailSender = new EmailSender(emailConfig);
            var messageHistory = new MessageHistory
            {
                To = string.Join(",", message.To.Select(x => x.Address)),
                Cc = string.Join(",", message.Cc.Select(x => x.Address)),
                Content = message.Content,
                Subject = message.Subject
            };
            NotificationService.AddNotificationHistory(messageHistory);
            emailSender.SendEmail(message);
            return true;
        }
    }
}
