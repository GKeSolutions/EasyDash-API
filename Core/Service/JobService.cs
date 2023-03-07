using Core.Enum;
using Core.Interface;
using Core.Model.Notification;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class JobService: IJobService
    {
        private IMissingTimeService MissingTimeService;
        private INotificationService NotificationService;
        private IDashboardService DashboardService;
        private readonly IConfiguration Configuration;
        private readonly ILogger<JobService> Logger;

        public JobService(IMissingTimeService missingTimeService, INotificationService notificationService, IDashboardService dashboardService, IConfiguration configuration , ILogger<JobService> logger) 
        {
            MissingTimeService = missingTimeService;
            DashboardService= dashboardService;
            NotificationService= notificationService;
            Configuration = configuration;
            Logger = logger;
        }
        public bool AddJob(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{nameof(JobService)} - {nameof(AddJob)}");
            if (scheduledNotification.EventType == (int)EventType.ActionList)
                RecurringJob.AddOrUpdate(scheduledNotification.Id.ToString(), () => SendEmailActionList(scheduledNotification), scheduledNotification.CronExpression);
            else if (scheduledNotification.EventType == (int)EventType.MissingTime)
                RecurringJob.AddOrUpdate(scheduledNotification.Id.ToString(), () => SendEmailMissingTime(scheduledNotification), scheduledNotification.CronExpression);
            return true;
        }

        public bool DeleteJob(string jobId)
        {
            Logger.LogInformation($"{nameof(JobService)} - {nameof(DeleteJob)} {jobId}");
            RecurringJob.RemoveIfExists(jobId);
            return true;
        }

        public async Task<string> SendEmailActionList(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{nameof(JobService)} - {nameof(SendEmailActionList)}");
            var users = await DashboardService.GetOpenProcessesPerTemplate(scheduledNotification.NotificationTemplate);
            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.UserEmail))
                {
                    var emailNotification = new EmailNotification
                    {
                        EmailAddress = user.UserEmail,
                        CcContact = await MissingTimeService.GetCcContactEmailAddress(scheduledNotification.CcContact),
                        NotificationTemplateId = scheduledNotification.NotificationTemplate,
                        EventType = (int)EventType.ActionList
                    };
                    var tags = BuildProcessTags(user.UserName, user.ProcessCaption, user.LastUpdated, user.ProcessItemId);
                    await NotificationService.SendEmailNotification(emailNotification, tags, true);
                }

            }

            return string.Empty;
        }

        public async Task<string> SendEmailMissingTime(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{nameof(JobService)} - {nameof(SendEmailMissingTime)}");
            var users = await MissingTimeService.GetMissingTimeUsersPerTemplate(scheduledNotification.NotificationTemplate);
            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.EmailAddress))
                {
                    var emailNotification = new EmailNotification
                    {
                        EmailAddress = user.EmailAddress,
                        CcContact = await MissingTimeService.GetCcContactEmailAddress(scheduledNotification.CcContact),
                        NotificationTemplateId = scheduledNotification.NotificationTemplate,
                        EventType = (int)EventType.MissingTime
                    };
                    var tags = BuildMissingTimeTags(user.UserName, user.WeekStartDate, user.WeeklyHoursRequired, user.WorkHrs);
                    await NotificationService.SendEmailNotification(emailNotification, tags, true);
                }
            }

            return string.Empty;
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
            tags["WeekName"] = weekName.ToString("MM/dd/yyyy");;
            tags["MissingHours"] = (requiredHours - loggedHours).ToString();
            tags["RequiredHours"] = requiredHours.ToString();
            tags["LoggedHours"] = loggedHours.ToString();
            return tags;
        }
    }
}
