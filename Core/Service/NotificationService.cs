﻿using Core.Interface;
using Core.Model.Notification;
using Dapper;
using Microsoft.Extensions.Configuration;
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
            return await connection.QueryAsync<Model.Notification.Type>("GeNotificationType");
        }

        #region Template
        public async Task<IEnumerable<Template>> GetTemplate()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Template>("GeNotificationTemplate");
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
                Priority = template.Priority,
                Role = template.Role,
                Process = template.Process,
                TemplateSubject = template.TemplateSubject,
                TemplateBody = template.TemplateBody
            });
            return await connection.ExecuteScalarAsync<Template>("CreateTemplate", dparam);
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
                Priority = template.Priority,
                Role = template.Role,
                Process = template.Process,
                TemplateSubject = template.TemplateSubject,
                TemplateBody = template.TemplateBody
            });
            return await connection.ExecuteScalarAsync<Template>("UpdateTemplate", dparam);
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
            return await connection.ExecuteAsync("DeleteTemplate", dparam);
        }
        #endregion

        #region Scheduler
        public async Task<IEnumerable<Scheduler>> GetScheduler()
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            return await connection.QueryAsync<Scheduler>("GeNotificationScheduler");
        }

        public async Task<Scheduler> CreateScheduler(Scheduler scheduler)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                IsActive = scheduler.IsActive,
                NotificationTemplate = scheduler.NotificationTemplate,
                Priority = scheduler.Schedule,
                Role = scheduler.NotifyAfterDays,
                Process = scheduler.ReassignTo,
                TemplateSubject = scheduler.CcContact
            });
            return await connection.ExecuteScalarAsync<Scheduler>("CreateScheduler", dparam);
        }

        public async Task<Scheduler> UpdateScheduler(Scheduler scheduler)
        {
            var connection = new SqlConnection(Configuration["ConnectionStrings:local"]);
            connection.Open();
            var dparam = new DynamicParameters();
            dparam.AddDynamicParams(new
            {
                Id = scheduler.Id,
                IsActive = scheduler.IsActive,
                NotificationTemplate = scheduler.NotificationTemplate,
                Priority = scheduler.Schedule,
                Role = scheduler.NotifyAfterDays,
                Process = scheduler.ReassignTo,
                TemplateSubject = scheduler.CcContact
            });
            return await connection.ExecuteScalarAsync<Scheduler>("UpdateScheduler", dparam);
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
            return await connection.ExecuteAsync("DeleteScheduler", dparam);
        }
        #endregion
    }
}
