namespace Core.Model.Notification
{
    public class MessageHistory
    {
        public string To { get; set; }
        public string Cc { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public int EventType { get; set; }
        public bool IsReassign { get; set; }
        public Guid ReassignTo { get; set; }
        public string ReassignToStr { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
        public Guid ProcItemId { get; set; }
        public string LastAccessTime { get; set; }
        public decimal RequiredHours { get; set; }
        public decimal LoggedHours { get; set; }
        public decimal MissingHours { get; set; }
        public bool IsManual { get; set; }
        public bool IsSystem { get; set; }
        public Guid TriggeredBy { get; set; }
        public string TriggeredByStr { get; set; }
        public string UserName { get; set; }
        public Guid UserId { get; set; }
        public DateTime openSince { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
