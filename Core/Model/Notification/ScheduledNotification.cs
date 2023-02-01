namespace Core.Model.Notification
{
    public class ScheduledNotification
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int NotificationTemplate { get; set; }
        public int Scheduler { get; set; }
        public int NotifyAfterDays { get; set; }
        public Guid ReassignTo { get; set; }
        public Guid CcContact { get; set; }
    }
}
