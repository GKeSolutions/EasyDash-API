namespace Core.Model.Notification
{
    public class EmailNotification
    {
        public Guid UserId { get; set; }
        public string EmailAddress { get; set; }
        public string CcContact { get; set; }
        public string ProcessCode { get; set; }
        public int EventType { get; set; }
        public bool IsReassign { get; set; }
        public Guid ReassignTo { get; set; }
        public bool IsManual { get; set; }
        public bool IsSystem { get; set; }
        public string TriggeredBy { get; set; }
        public int NotificationTemplateId { get; set; }
    }
}
