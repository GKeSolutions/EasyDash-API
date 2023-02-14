using Core.Enum;
using Core.Interface;
using Core.Model.Notification;
using Hangfire;

namespace Core.Service
{
    public class JobService: IJobService
    {
        private IMissingTimeService MissingTimeService;
        private INotificationService NotificationService;
        private IDashboardService DashboardService;

        public JobService(IMissingTimeService missingTimeService, INotificationService notificationService, IDashboardService dashboardService) 
        {
            MissingTimeService = missingTimeService;
            DashboardService= dashboardService;
        }
        public bool AddJob(ScheduledNotification scheduledNotification)
        {
            if(scheduledNotification.EventType == EventType.ActionList)
                RecurringJob.AddOrUpdate(scheduledNotification.Id.ToString(), () => SendEmailActionList(scheduledNotification), scheduledNotification.CronExpression);
            else if (scheduledNotification.EventType == EventType.MissingTime)
                RecurringJob.AddOrUpdate(scheduledNotification.Id.ToString(), () => SendEmailMissingTime(scheduledNotification), scheduledNotification.CronExpression);
            return true;
        }

        public bool DeleteJob(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);
            return true;
        }

        public async Task<string> SendEmailActionList(ScheduledNotification scheduledNotification)
        {
            var users = await DashboardService.GetOpenProcessesPerTemplate(scheduledNotification.NotificationTemplate);
            foreach (var user in users)
            {
                var emailNotification = new EmailNotification
                {
                    EmailAddress = user.UserEmail,
                    CcContact = "gilbert.khoury@gkesolutions.com"//await MissingTimeService.GetCcContactEmailAddress(scheduledNotification.CcContact)
                };
                await NotificationService.SendEmailNotification(emailNotification);
            }

            return string.Empty;
        }

        private async Task<string> SendEmailMissingTime(ScheduledNotification scheduledNotification)
        {
            var users = await MissingTimeService.GetMissingTimeUsersPerTemplate(scheduledNotification.NotificationTemplate);
            foreach (var user in users)
            {
                var emailNotification = new EmailNotification
                {
                    EmailAddress = user.EmailAddress,
                    CcContact = "gilbert.khoury@gkesolutions.com"//await MissingTimeService.GetCcContactEmailAddress(scheduledNotification.CcContact)
                };
                await NotificationService.SendEmailNotification(emailNotification);
            }

            return string.Empty;
        }
    }
}
