using Core.Enum;
using Core.Interface;
using Core.Model.Notification;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Notify : ControllerBase
    {
        private readonly INotificationService NotificationService;
        private readonly IDashboardService DashboardService;
        private readonly IMissingTimeService MissingTimeService;

        public Notify(INotificationService notificationService, IDashboardService dashboardService, IMissingTimeService missingTimeService)
        {
            NotificationService = notificationService;
            DashboardService = dashboardService;
            MissingTimeService = missingTimeService;
        }

        [HttpPost]
        public async Task<bool> Process([FromBody] ProcessNotification processNotification)
        {
            if (processNotification.ProcessCode is not null || processNotification.UserId != Guid.Empty)
            {
                if (processNotification.ProcessCode is not null && processNotification.UserId != Guid.Empty)//Single Notify-User clicked on the notify button
                    return await NotificationService.SendEmailNotification(new EmailNotification { EmailAddress=processNotification.EmailAddress,EventType=EventType.ActionList.ToString(),ProcessCode=processNotification.ProcessCode,UserId=processNotification.UserId});
                else if (processNotification.ProcessCode is null)//User NotifyAll
                {
                    var processes = await DashboardService.GetProcessesByUser(processNotification.UserId);
                    foreach (var process in processes)
                    {
                        var notification = new EmailNotification
                        {
                            EmailAddress = processNotification.EmailAddress,
                            ProcessCode = process.ProcessCode,
                            EventType = EventType.ActionList.ToString(),
                            UserId = processNotification.UserId
                        };
                        await NotificationService.SendEmailNotification(notification);
                    }
                }
                else //process notify all
                {
                    var users = await DashboardService.GetUsersByProcess(processNotification.ProcessCode);
                    foreach (var user in users)
                    {
                        var notification = new EmailNotification
                        {
                            EmailAddress = user.UserEmail,
                            ProcessCode = processNotification.ProcessCode,
                            EventType = EventType.ActionList.ToString(),
                            UserId = user.UserId
                        };
                        await NotificationService.SendEmailNotification(notification);
                    }
                }
            }
            return true;
        }

        [HttpPost]
        public async Task<bool> MissingTime([FromBody] MissingTimeNotification missingTimeNotification)
        {
            if (missingTimeNotification.IsOneWeek)
            {
                if (missingTimeNotification.IsOneUser)
                {
                    var missingTime = await MissingTimeService.GetTimePerUserPerWeek(missingTimeNotification.UserId, missingTimeNotification.StartDate, missingTimeNotification.EndDate);
                    if (missingTime.WorkHrs < missingTime.WeeklyHoursRequired)
                    {
                        var notification = new EmailNotification
                        {
                            EmailAddress = missingTimeNotification.UserEmail,
                            EventType = EventType.MissingTime.ToString(),
                            UserId = missingTimeNotification.UserId,
                        };
                        await NotificationService.SendEmailNotification(notification);
                    }
                }
                else
                {
                    var missingTimeUsers = await MissingTimeService.GetUsersTimePerWeek(missingTimeNotification.StartDate, missingTimeNotification.EndDate);
                    foreach (var user in missingTimeUsers)
                    {
                        if (user.WorkHrs < user.WeeklyHoursRequired)
                        {
                            var notification = new EmailNotification
                            {
                                EmailAddress = user.EmailAddress,
                                EventType = EventType.MissingTime.ToString(),
                                UserId = user.UserId,
                            };
                            await NotificationService.SendEmailNotification(notification);
                        }
                    }
                }
            }
            else
            {
                var weeks = await MissingTimeService.GetWeeksTimePerUser(missingTimeNotification.UserId, missingTimeNotification.StartDate, missingTimeNotification.EndDate);
                foreach (var week in weeks)
                {
                    if (week.WorkHrs < week.WeeklyHoursRequired)
                    {
                        var notification = new EmailNotification
                        {
                            EmailAddress = missingTimeNotification.UserEmail,
                            EventType = EventType.MissingTime.ToString(),
                            UserId = missingTimeNotification.UserId,
                        };
                        await NotificationService.SendEmailNotification(notification);
                    }
                }
            }
            return true;
        }
    }
}
