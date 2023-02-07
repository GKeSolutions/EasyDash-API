namespace Core.Model.Notification
{
    public class EmailNotification
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string CcContact { get; set; }
        public string ProcessCode { get; set; }
        public string EventType { get; set; }
    }
}
