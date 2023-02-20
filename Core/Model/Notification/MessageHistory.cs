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
        public bool IsManual { get; set; }
        public bool IsSystem { get; set; }
        public string TriggeredBy { get; set; }
    }
}
