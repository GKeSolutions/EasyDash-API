namespace Core.Model.Notification
{
    public class EmailNotification
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Process { get; set; }
        public string EventType { get; set; }
    }
}
