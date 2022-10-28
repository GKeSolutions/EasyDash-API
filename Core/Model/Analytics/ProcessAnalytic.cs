namespace Core.Model.Analytics
{
    public class ProcessAnalytic
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        public double? AverageHours { get; set; }
        public double? TotalHours { get; set; }
        public IEnumerable<UserAnalytic> Users { get; set; }
    }
}
