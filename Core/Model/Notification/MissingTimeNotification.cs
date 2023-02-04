namespace Core.Model.Notification
{
    public class MissingTimeNotification
    {
        public Guid UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsOneWeek { get; set; }
        public bool IsOneUser { get; set; }
        public string UserEmail { get; set; }
    }
}
