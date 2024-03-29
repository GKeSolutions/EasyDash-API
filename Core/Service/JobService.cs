﻿using Core.Enum;
using Core.Interface;
using Core.Model.Notification;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Service
{
    public class JobService: IJobService
    {
        private IMissingTimeService MissingTimeService;
        private INotificationService NotificationService;
        private IProcessService ProcessService;
        private IReassignService ReassignService;
        private readonly IConfiguration Configuration;
        private IHttpContextAccessor HttpContextAccessor;
        private readonly ILogger<JobService> Logger;
        private string UserName;

        public JobService(IMissingTimeService missingTimeService, INotificationService notificationService, IProcessService processService, IReassignService reassignService, IConfiguration configuration , ILogger<JobService> logger, IHttpContextAccessor httpContextAccessor) 
        {
            MissingTimeService = missingTimeService;
            ProcessService = processService;
            NotificationService= notificationService;
            ReassignService= reassignService;
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            Logger = logger;
            UserName = HttpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }
        public bool AddJob(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{UserName} - {nameof(JobService)} - {nameof(AddJob)}");
            if (scheduledNotification.EventType == (int)EventType.ActionList)
                RecurringJob.AddOrUpdate(scheduledNotification.Id.ToString(), () => ProcessAcctionList(scheduledNotification), scheduledNotification.CronExpression);
            else if (scheduledNotification.EventType == (int)EventType.MissingTime)
                RecurringJob.AddOrUpdate(scheduledNotification.Id.ToString(), () => SendEmailMissingTime(scheduledNotification), scheduledNotification.CronExpression);
            return true;
        }

        public bool DeleteJob(string jobId)
        {
            Logger.LogInformation($"{UserName} - {nameof(JobService)} - {nameof(DeleteJob)} {jobId}");
            RecurringJob.RemoveIfExists(jobId);
            return true;
        }

        public async Task<string> ProcessAcctionList(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{nameof(JobService)} - {nameof(ProcessAcctionList)}");
            var users = await ProcessService.GetOpenProcessesPerTemplate(scheduledNotification.NotificationTemplate);
            foreach (var user in users)
            {
                if (string.IsNullOrEmpty(user.UserEmail))
                {
                    Logger.LogInformation($"{nameof(JobService)} - {nameof(ProcessAcctionList)} - Skipped sending email to {user.UserId} - Missing Email Address");
                    continue;
                }
                if (!string.IsNullOrEmpty(user.UserEmail))
                {
                    var emailNotification = new EmailNotification
                    {
                        EmailAddress = user.UserEmail,
                        CcContact = await MissingTimeService.GetCcContactEmailAddress(scheduledNotification.CcContact),
                        NotificationTemplateId = scheduledNotification.NotificationTemplate,
                        EventType = (int)EventType.ActionList,
                        IsSystem = true,
                        UserId = user.UserId,
                        ProcItemId = user.ProcessItemId,
                        ProcessCode = user.ProcessCode,
                        ProcessDescription = user.ProcessDescription,
                        LastAccessTime = user.LastUpdated
                    };
                    var tags = BuildProcessTags(user.UserName, user.ProcessCaption, user.LastUpdated, user.ProcessItemId);
                    await NotificationService.SendEmailNotification(emailNotification, tags, true);
                }
                if (scheduledNotification.ReassignTo != Guid.Empty)
                {
                    Logger.LogInformation($"{nameof(JobService)} - {nameof(ProcessAcctionList)} - Reassiging Process {user.ProcessItemId} with process code {user.ProcessCode} to user {scheduledNotification.ReassignTo}");
                    await ReassignService.Reassign(user.ProcessCode, user.ProcessItemId, scheduledNotification.ReassignTo.Value, true);
                }
            }

            return string.Empty;
        }

        public async Task<string> SendEmailMissingTime(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{UserName} - {nameof(JobService)} - {nameof(SendEmailMissingTime)}");
            var users = await MissingTimeService.GetMissingTimeUsersPerTemplate(scheduledNotification.NotificationTemplate);
            foreach (var user in users)
            {
                if (!string.IsNullOrEmpty(user.EmailAddress) || Configuration["AppConfiguration:IsTestMode"] == "1")
                {
                    var emailNotification = new EmailNotification
                    {
                        EmailAddress = user.EmailAddress,
                        CcContact = await MissingTimeService.GetCcContactEmailAddress(scheduledNotification.CcContact),
                        NotificationTemplateId = scheduledNotification.NotificationTemplate,
                        EventType = (int)EventType.MissingTime,
                        IsSystem = true,
                        UserId = user.UserId,
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
