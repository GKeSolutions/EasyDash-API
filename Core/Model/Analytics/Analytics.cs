namespace Core.Model.Analytics
{
    public class Analytics
    {
        public string ProcessCode { get; set; }
        public string ProcessName { get; set; }
        //public Guid ProcItemId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        //public string StepIdStr { get; set; }
        //public Guid ProcStepId { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public int AvgTimeSpentInhours { get; set; }
        public int AvgTimeSpentInMinutes { get; set; }
    }
}
