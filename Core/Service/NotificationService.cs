using Core.Enum;
using Core.Interface;
using Core.Model.Notification;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Notification;
using System.Data.SqlClient;

namespace Core.Service
{
    public class NotificationService : INotificationService
    {
        private IConfiguration Configuration;
        private bool IsTestMode = true;
        private string TestEmails;
        private readonly ILogger<NotificationService> Logger;
        private IHttpContextAccessor HttpContextAccessor;
        private string UserName;

        public NotificationService(IConfiguration configuration, ILogger<NotificationService> logger, IHttpContextAccessor httpContextAccessor)
        {
            Configuration = configuration;
            IsTestMode = Configuration["AppConfiguration:IsTestMode"] == "1";
            TestEmails = Configuration["AppConfiguration:TestEmails"];
            Logger = logger;
            HttpContextAccessor = httpContextAccessor;
            UserName = HttpContextAccessor?.HttpContext?.User?.Identity?.Name;
        }
        public async Task<IEnumerable<Model.Notification.Type>> GetType()
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(GetType)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Model.Notification.Type>("ed.GeNotificationType");
        }

        #region Template
        public async Task<IEnumerable<Template>> GetTemplate()
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(GetTemplate)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Template>("ed.GeNotificationTemplate");
        }

        public async Task<Template> CreateTemplate(Template template)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(CreateTemplate)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                IsActive = template.IsActive,
                Description = template.Description,
                Type = template.Type,
                Priority = template.Priority,
                Role = template.Role,
                Process = template.Process,
                TemplateSubject = template.TemplateSubject,
                TemplateBody = template.TemplateBody
            });
            return await connection.QueryFirstOrDefaultAsync<Template>("ed.CreateTemplate", param:dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Template> UpdateTemplate(Template template)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(UpdateTemplate)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = template.Id,
                IsActive = template.IsActive,
                Description = template.Description,
                Type = template.Type,
                Priority = template.Priority,
                Role = template.Role,
                Process = template.Process,
                TemplateSubject = template.TemplateSubject,
                TemplateBody = template.TemplateBody
            });
            return await connection.QueryFirstOrDefaultAsync<Template>("ed.UpdateTemplate", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> DeleteTemplate(int template)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(DeleteTemplate)} {template}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = template
            });
            return await connection.ExecuteAsync("ed.DeleteTemplate", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }
        #endregion

        #region ScheduledNotification
        public async Task<IEnumerable<ScheduledNotification>> GetScheduledNotification()
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(GetScheduledNotification)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<ScheduledNotification>("ed.GeNotificationScheduler");
        }

        public async Task<ScheduledNotification> CreateScheduledNotification(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(CreateScheduledNotification)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                scheduledNotification.IsActive,
                scheduledNotification.NotificationTemplate,
                scheduledNotification.Scheduler,
                scheduledNotification.NotifyAfterDays,
                scheduledNotification.ReassignTo,
                scheduledNotification.CcContact
            });
            return await connection.QueryFirstOrDefaultAsync<ScheduledNotification>("ed.CreateScheduledNotification", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<ScheduledNotification> UpdateScheduledNotification(ScheduledNotification scheduledNotification)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(UpdateScheduledNotification)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                scheduledNotification.Id,
                scheduledNotification.IsActive,
                scheduledNotification.NotificationTemplate,
                Priority = scheduledNotification.Scheduler,
                Role = scheduledNotification.NotifyAfterDays,
                Process = scheduledNotification.ReassignTo,
                TemplateSubject = scheduledNotification.CcContact
            });
            return await connection.QueryFirstOrDefaultAsync<ScheduledNotification>("ed.UpdateScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> DeleteScheduledNotification(int scheduledNotification)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(DeleteScheduledNotification)} {scheduledNotification}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = scheduledNotification
            });
            return await connection.ExecuteAsync("ed.DeleteScheduledNotification", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }
        #endregion

        #region Scheduler
        public async Task<IEnumerable<Scheduler>> GetScheduler()
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(GetScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Scheduler>("ed.GeScheduler");
        }

        public async Task<Scheduler> CreateScheduler(Scheduler scheduler)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(CreateScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                CronExpression = scheduler.CronExpression,
                Description = scheduler.Description,
            });
            return await connection.QueryFirstOrDefaultAsync<Scheduler>("ed.CreateScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Scheduler> UpdateScheduler(Scheduler scheduler)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(UpdateScheduler)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {   Id=scheduler.Id,
                CronExpression = scheduler.CronExpression,
                Description = scheduler.Description
            });
            return await connection.QueryFirstOrDefaultAsync<Scheduler>("ed.UpdateScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> DeleteScheduler(int scheduler)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(DeleteScheduler)} {scheduler}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = scheduler
            });
            return await connection.ExecuteAsync("ed.DeleteScheduler", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }
        #endregion

        public async Task<NotificationInfo> GetNotificationInfo(EmailNotification notification)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(GetNotificationInfo)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                UserId = notification.UserId,
                Process = notification.ProcessCode,
                EventType=notification.EventType
            });
            return await connection.QueryFirstOrDefaultAsync<NotificationInfo>("ed.GetUserNotificationTemplate", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<NotificationInfo> GetNotificationTemplateInfo(int notificationTemplateId)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(GetNotificationTemplateInfo)} {notificationTemplateId}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                NotificationTemplateId = notificationTemplateId
            });
            return await connection.QueryFirstOrDefaultAsync<NotificationInfo>("ed.GetNotificationTemplateInfo", param: dparam, commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<int> AddNotificationHistory(MessageHistory message)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(AddNotificationHistory)}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                message.To,
                message.Cc,
                message.Subject,
                message.Content,
                message.EventType,
                message.IsManual,
                message.IsReassign,
                message.IsSystem,
                message.ReassignTo,
                message.ProcessCode,
                message.ProcessDescription,
                message.ProcItemId,
                message.LastAccessTime,
                message.MissingHours,
                message.RequiredHours,
                message.LoggedHours,
                message.TriggeredBy,
                message.UserId
            });
            return await connection.ExecuteAsync("ed.AddNotificationHistory", param: dparam, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<IEnumerable<MessageHistory>> GetNotificationHistory(NotificationHistoryFilter filter)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(GetNotificationHistory)} {filter.ActionType}");
            using var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                ActionType = filter.ActionType,
                UserId = filter.UserId,
                StartDate = filter.StartDate,
                EndDate = filter.EndDate,
                ProcessCode = filter.ProcessCode,
                ProcItemId = filter.ProcItemId
            });
            return await connection.QueryAsync<MessageHistory>("ed.GetNotificationHistory", param: dparam, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<bool> SendEmailNotification(EmailNotification emailNotification, Dictionary<string, string> tags, bool isSystemJob=false)
        {
            Logger.LogInformation($"{UserName} - {nameof(NotificationService)} - {nameof(SendEmailNotification)}");
            NotificationInfo info;
            if (isSystemJob)
                info = await GetNotificationTemplateInfo(emailNotification.NotificationTemplateId);
            else
                info = await GetNotificationInfo(emailNotification);

            if (info is null) return false; else info.TemplateBody = ReplaceTags(info.TemplateBody, emailNotification.EventType, tags);
            var message = new Message(new string[] { emailNotification.EmailAddress }, emailNotification.CcContact is null ? null : new string[] { emailNotification.CcContact }, info.TemplateSubject, info.TemplateBody, IsTestMode, TestEmails.Split(","));
            var emailConfig = Configuration
                .GetSection("EmailConfiguration")
                .Get<EmailConfiguration>();
            var emailSender = new EmailSender(emailConfig);
            var messageHistory = new MessageHistory
            {
                To = string.Join(",", message.To.Select(x => x.Address)),
                Cc = message.To.Select(x => x.Address).First(),
                Content = message.Content,
                Subject = message.Subject,
                EventType = emailNotification.EventType,
                IsManual = emailNotification.IsManual,
                IsReassign = emailNotification.IsReassign,
                ProcessCode= emailNotification.ProcessCode,
                ProcessDescription = emailNotification.ProcessDescription,
                LastAccessTime = emailNotification.LastAccessTime,
                LoggedHours = emailNotification.LoggedHours,
                MissingHours = emailNotification.MissingHours,
                ProcItemId = emailNotification.ProcItemId,
                RequiredHours = emailNotification.RequiredHours,
                IsSystem = isSystemJob,
                ReassignTo = emailNotification.ReassignTo,
                TriggeredBy = emailNotification.TriggeredBy,
                UserId = emailNotification.UserId,
            };
            await AddNotificationHistory(messageHistory);
            return await emailSender.SendEmail(message);
        }

        private string ReplaceTags(string template, int eventType, Dictionary<string, string> tags)
        {
            if(eventType==(int)EventType.ActionList)
                return template.Replace("@UserName", tags["UserName"]).Replace("@ProcessName", "'" + tags["ProcessCaption"] + "'").Replace("@LastAccessTime", tags["LastUpdated"]).Replace("@ProcessLink", tags["ProcessLink"]);
            else
                return template.Replace("@UserName", tags["UserName"]).Replace("@WeekName", tags["WeekName"]).Replace("@MissingHours", tags["MissingHours"]).Replace("@LoggedHours", tags["LoggedHours"]).Replace("@RequiredHours", tags["RequiredHours"]);
        }
    }
}
