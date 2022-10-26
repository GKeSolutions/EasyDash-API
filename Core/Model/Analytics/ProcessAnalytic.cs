namespace Core.Model.Analytics
{
    public class ProcessAnalytic
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        //public Guid ProcItemId { get; set; }
        public decimal AverageHours { get; set; }
        public IEnumerable<UserAnalytic> Users { get; set; }
    }
}
