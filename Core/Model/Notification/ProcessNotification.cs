namespace Core.Model.Notification
{
    public class ProcessNotification
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessId { get; set; }
    }
}
