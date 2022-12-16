﻿using Core.Model.Notification;

namespace Core.Interface;

public interface INotificationService
{
    Task<IEnumerable<Model.Notification.Type>> GetType();
    Task<IEnumerable<Template>> GetTemplate();
    Task<Template> CreateTemplate(Template template);
    Task<Template> UpdateTemplate(Template template);
    Task<int> DeleteTemplate(int template);
}
