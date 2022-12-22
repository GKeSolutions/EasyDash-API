using Microsoft.AspNetCore.Mvc;
using Notification;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Notify : ControllerBase
    {
        private readonly IConfiguration Configuration;
        public Notify(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        [HttpGet]
        public bool Send()
        {
            var rng = new Random();
            var message = new Message(new string[] { "joe.doumit1@gmail.com" }, "Test email", "This is the content from our email.");
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            var emailSender = new EmailSender(emailConfig);
            emailSender.SendEmail(message);
            return true;
        }
    }
}
