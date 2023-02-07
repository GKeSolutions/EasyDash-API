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
        public JobService(IMissingTimeService missingTimeService, INotificationService notificationService) 
        {
            MissingTimeService = missingTimeService;
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

        private string SendEmailActionList(ScheduledNotification scheduledNotification)
        {
            return string.Empty;
        }

        private async Task<string> SendEmailMissingTime(ScheduledNotification scheduledNotification)
        {
            //Check if missing during last week and send them email
            //for this template id, get the role, and for this role we get the users
            //GetMissingTimeUsersPerTemplate
            var users = await MissingTimeService.GetMissingTimeUsersPerTemplate(scheduledNotification.NotificationTemplate);
            foreach (var user in users)
            {
                var emailNotification = new EmailNotification
                {
                    EmailAddress = user.EmailAddress,
                    CcContact = await MissingTimeService.GetCcContactEmailAddress(scheduledNotification.CcContact)
                };
                await NotificationService.SendEmailNotification(emailNotification);
            }


            return string.Empty;
        }
    }
}
