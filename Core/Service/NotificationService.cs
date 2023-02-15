﻿using Core.Enum;
using Core.Interface;
using Core.Model.Notification;
using Dapper;
using Microsoft.Extensions.Configuration;
using Notification;
using System.Data.SqlClient;

namespace Core.Service
{
    public class NotificationService : INotificationService
    {
        private IConfiguration Configuration;
        public NotificationService(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<IEnumerable<Model.Notification.Type>> GetType()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Model.Notification.Type>("ed.GeNotificationType");
        }

        #region Template
        public async Task<IEnumerable<Template>> GetTemplate()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Template>("ed.GeNotificationTemplate");
        }

        public async Task<Template> CreateTemplate(Template template)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<ScheduledNotification>("ed.GeNotificationScheduler");
        }

        public async Task<ScheduledNotification> CreateScheduledNotification(ScheduledNotification scheduledNotification)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Scheduler>("ed.GeScheduler");
        }

        public async Task<Scheduler> CreateScheduler(Scheduler scheduler)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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

        public async Task<int> AddNotificationHistory(MessageHistory message)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
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
                message.TriggeredBy
            });
            return await connection.ExecuteAsync("ed.AddNotificationHistory", param: dparam, commandType: System.Data.CommandType.StoredProcedure);

        }

        public async Task<bool> SendEmailNotification(EmailNotification emailNotification, Dictionary<string, string> tags)
        {
            var info = await GetNotificationInfo(emailNotification);

            if (info is null) return false; else info.TemplateBody = ReplaceTags(info.TemplateBody, emailNotification.EventType, tags);
            var message = new Message(new string[] { emailNotification.EmailAddress }, emailNotification.CcContact is null ? null : new string[] { emailNotification.CcContact }, info.TemplateSubject, info.TemplateBody);
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
                IsSystem = emailNotification.IsSystem,
                ReassignTo = emailNotification.ReassignTo,
                TriggeredBy = emailNotification.TriggeredBy
            };
            await AddNotificationHistory(messageHistory);
            emailSender.SendEmail(message);

            return true;
        }

        private string ReplaceTags(string template, string eventType, Dictionary<string, string> tags)
        {
            if(eventType==EventType.ActionList.ToString())
                return template.Replace("@UserName", tags["UserName"]).Replace("@ProcessName", tags["ProcessCaption"]);
            else
                return template.Replace("@UserName", "joed").Replace("@ProcessName", "test process").Replace("@WeekName", "test week").Replace("@MissingHours", "10").Replace("@LastAccessTime", "2002-01-01");
        }
    }
}
