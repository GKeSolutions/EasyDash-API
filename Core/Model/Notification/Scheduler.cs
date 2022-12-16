namespace Core.Model.Notification
{
    public class Scheduler
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int NotificationTemplate { get; set; }
        public string Schedule { get; set; }
        public int NotifyAfterDays { get; set; }
        public Guid ReassignTo { get; set; }
        public Guid CcContact { get; set; }
    }
}
