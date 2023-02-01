namespace Core.Model.Notification
{
    public class Scheduler
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CronExpression { get; set; }
    }
}
