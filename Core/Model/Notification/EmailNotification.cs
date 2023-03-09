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
        public Guid TriggeredBy { get; set; }
        public int NotificationTemplateId { get; set; }
        public string ProcessDescription { get; set; }
        public Guid ProcItemId { get; set; }
        public string LastAccessTime { get; set; }
        public decimal RequiredHours { get; set; }
        public decimal LoggedHours { get; set; }
        public decimal MissingHours { get; set; }
    }
}
