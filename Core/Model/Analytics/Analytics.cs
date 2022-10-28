namespace Core.Model.Analytics
{
    public class Analytics
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public int AvgTimeSpentInhours { get; set; }
        public int AvgTimeSpentInMinutes { get; set; }
        public int TotalTimeSpentInHours { get; set; }
        public int TotalTimeSpentInMinutes { get; set; }
    }
}
