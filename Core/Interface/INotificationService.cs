﻿using Core.Model.Notification;

namespace Core.Interface;

public interface INotificationService
{
    Task<IEnumerable<Model.Notification.Type>> GetType();
    Task<IEnumerable<Template>> GetTemplate();
    Task<Template> CreateTemplate(Template template);
    Task<Template> UpdateTemplate(Template template);
    Task<int> DeleteTemplate(int template);
    Task<NotificationInfo> GetNotificationInfo(EmailNotification notification);
    Task<int> AddNotificationHistory(MessageHistory messageHistory);
    Task<IEnumerable<MessageHistory>> GetNotificationHistory(NotificationHistoryFilter filter);
    Task<bool> SendEmailNotification(EmailNotification emailNotification, Dictionary<string, string> tags, bool isSystemJob = false);
    Task<NotificationInfo> GetNotificationTemplateInfo(int notificationTemplateId);
}

