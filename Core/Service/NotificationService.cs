using Core.Interface;

namespace Core.Service
{
    public class NotificationService : INotificationService
    {
        public List<Model.Notification.Type> Type()
        {
            return new List<Model.Notification.Type>
            {
                new Model.Notification.Type { Id = 1, Name = "Action List" },
                new Model.Notification.Type { Id = 2, Name = "Missing Time" }
            };
        }
    }
}
