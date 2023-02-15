using Core.Enum;
using Core.Interface;
using Core.Model.Analytics;
using Core.Model.Dashboard.User;
using Core.Model.MissingTime;
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
                {
                    var info = await DashboardService.GetProcessInfoByProcId(processNotification.ProcessId);
                    var tags = new Dictionary<string, string>();
                    tags["UserName"] = info.UserName;
                    tags["ProcessCaption"] = info.ProcessCaption;
                    return await NotificationService.SendEmailNotification(new EmailNotification { EmailAddress = processNotification.EmailAddress, CcContact = processNotification.CcContact, EventType = EventType.ActionList.ToString(), ProcessCode = processNotification.ProcessCode, UserId = processNotification.UserId }, tags);

                }
                else if (processNotification.ProcessCode is null)//User NotifyAll
                {
                    var processes = await DashboardService.GetProcessesByUser(processNotification.UserId);
                    foreach (var process in processes)
                    {
                        var notification = new EmailNotification
                        {
                            EmailAddress = processNotification.EmailAddress,
                            CcContact = processNotification.CcContact,
                            ProcessCode = process.ProcessCode,
                            EventType = EventType.ActionList.ToString(),
                            UserId = processNotification.UserId
                        };
                        var tags = new Dictionary<string, string>();
                        tags["UserName"] = process.UserName;
                        tags["ProcessCaption"] = process.ProcessCaption;
                        await NotificationService.SendEmailNotification(notification, tags);
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
                            CcContact = processNotification.CcContact,
                            ProcessCode = processNotification.ProcessCode,
                            EventType = EventType.ActionList.ToString(),
                            UserId = user.UserId
                        };
                        var tags = new Dictionary<string, string>();
                        tags["UserName"] = user.UserName;
                        tags["ProcessCaption"] = user.ProcessCaption;
                        await NotificationService.SendEmailNotification(notification, tags);
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
                            CcContact = missingTimeNotification.CcContact,
                            EventType = EventType.MissingTime.ToString(),
                            UserId = missingTimeNotification.UserId,
                        };
                        var tags = new Dictionary<string, string>();
                        tags["UserName"] = missingTime.UserName;
                        //tags["WeekName"] = missingTime.WeekName;
                        // tags["MissingHours"] = missingTime.MissingHours;
                        await NotificationService.SendEmailNotification(notification, tags);
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
                                CcContact = missingTimeNotification.CcContact,
                                EventType = EventType.MissingTime.ToString(),
                                UserId = user.UserId,
                            };
                            var tags = new Dictionary<string, string>();
                            tags["UserName"] = user.UserName;
                            //tags["WeekName"] = user.WeekName;
                            // tags["MissingHours"] = user.MissingHours;
                            await NotificationService.SendEmailNotification(notification, tags);
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
                            CcContact = missingTimeNotification.CcContact,
                            EventType = EventType.MissingTime.ToString(),
                            UserId = missingTimeNotification.UserId,
                        };
                        var tags = new Dictionary<string, string>();
                        tags["UserName"] = week.UserName;
                        //tags["WeekName"] = user.WeekName;
                        // tags["MissingHours"] = user.MissingHours;
                        await NotificationService.SendEmailNotification(notification, tags);
                    }
                }
            }
            return true;
        }
    }
}
