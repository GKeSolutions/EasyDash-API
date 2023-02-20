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
        private readonly IConfiguration Configuration;

        public Notify(INotificationService notificationService, IDashboardService dashboardService, IMissingTimeService missingTimeService, IConfiguration configuration)
        {
            NotificationService = notificationService;
            DashboardService = dashboardService;
            MissingTimeService = missingTimeService;
            Configuration = configuration;
        }

        [HttpPost]
        public async Task<bool> Process([FromBody] ProcessNotification processNotification)
        {
            if(processNotification.ProcessId != null)
            {
                var info = await DashboardService.GetProcessInfoByProcId(processNotification.ProcessId);
                var tags = BuildProcessTags(info.UserName, info.ProcessCaption, info.LastUpdated, info.ProcessItemId);
                return await NotificationService.SendEmailNotification(new EmailNotification { EmailAddress = info.UserEmail, CcContact = processNotification.CcContact, EventType = (int)EventType.ActionList, ProcessCode = processNotification.ProcessCode, UserId = processNotification.UserId }, tags);
            }
            else if (processNotification.UserId != Guid.Empty)//User NotifyAll
            {
                var processes = await DashboardService.GetProcessesByUser(processNotification.UserId);
                foreach (var process in processes)
                {
                    if (process.Users != null || process?.Users?.Count != 0)
                    {
                        if (!string.IsNullOrEmpty(process?.Users?.FirstOrDefault()?.UserEmail))
                        {
                            var notification = new EmailNotification
                            {
                                EmailAddress = process?.Users?.FirstOrDefault()?.UserEmail,
                                CcContact = processNotification.CcContact,
                                ProcessCode = process?.ProcessCode,
                                EventType = (int)EventType.ActionList,
                                UserId = processNotification.UserId
                            };
                            var tags = BuildProcessTags(process.UserName, process.ProcessCaption, process.LastUpdated, process.ProcessItemId);
                            await NotificationService.SendEmailNotification(notification, tags);
                        }
                    }
                }
            }
            else if(processNotification.ProcessCode != null) //process notify all
            {
                var processItems = await DashboardService.GetProcessItemsByProcessCode(processNotification.ProcessCode);
                foreach (var processItem in processItems)
                {
                    var notification = new EmailNotification
                    {
                        EmailAddress = processItem.UserEmail,
                        CcContact = processNotification.CcContact,
                        ProcessCode = processNotification.ProcessCode,
                        EventType = (int)EventType.ActionList,
                        UserId = processItem.UserId
                    };
                    var tags = BuildProcessTags(processItem.UserName, processItem.ProcessCaption, processItem.LastUpdated, processItem.ProcessItemId);
                    await NotificationService.SendEmailNotification(notification, tags);
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
                            EmailAddress = missingTime.EmailAddress,
                            CcContact = missingTimeNotification.CcContact,
                            EventType = (int)EventType.MissingTime,
                            UserId = missingTimeNotification.UserId,
                        };
                        var tags = BuildMissingTimeTags(missingTime.UserName, missingTimeNotification.StartDate, missingTime.WeeklyHoursRequired, missingTime.WorkHrs);
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
                                EventType = (int)EventType.MissingTime,
                                UserId = user.UserId,
                            };
                            var tags = BuildMissingTimeTags(user.UserName, missingTimeNotification.StartDate, user.WeeklyHoursRequired, user.WorkHrs);
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
                            EmailAddress = week.EmailAddress,
                            CcContact = missingTimeNotification.CcContact,
                            EventType = (int)EventType.MissingTime  ,
                            UserId = missingTimeNotification.UserId,
                        };
                        var tags = BuildMissingTimeTags(week.UserName, week.WeekStartDate, week.WeeklyHoursRequired, week.WorkHrs);
                        await NotificationService.SendEmailNotification(notification, tags);
                    }
                }
            }
            return true;
        }

        private Dictionary<string, string> BuildProcessTags(string userName, string processCaption, string lastUpdated, Guid processItemId)
        {
            var tags = new Dictionary<string, string>();
            tags["UserName"] = userName;
            tags["ProcessCaption"] = processCaption;
            tags["LastUpdated"] = lastUpdated;
            tags["ProcessLink"] = "<a href=" + Configuration["InstanceConfiguration:BaseUrl"] + "Process/" + processItemId + "> Process Link</a> ";
            return tags;
        }

        private Dictionary<string, string> BuildMissingTimeTags(string userName, DateTime weekName, decimal requiredHours, int loggedHours)
        {
            var tags = new Dictionary<string, string>();
            tags["UserName"] = userName;
            tags["WeekName"] = weekName.ToString("MM/dd/yyyy");
            tags["MissingHours"] = (requiredHours - loggedHours).ToString();
            tags["RequiredHours"] = requiredHours.ToString();
            tags["LoggedHours"] = loggedHours.ToString();
            return tags;
        }
    }
}
