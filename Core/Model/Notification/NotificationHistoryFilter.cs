namespace Core.Model.Notification
{
    public class NotificationHistoryFilter
    {
        public int ActionType { get; set; }
        public Guid? UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string ProcessCode { get; set; }
        public Guid? ProcItemId { get; set; }
    }
}
