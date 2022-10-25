namespace Core.Model.Analytics
{
    public class UserAnalytic
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<ProcessAnalytic> Processes { get; set; }
        public decimal AverageHours { get; set; }
    }
}
