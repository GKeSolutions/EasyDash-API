namespace Core.Model.MissingTime
{
    public class Time
    {
        public Guid UserId{ get; set; }
        public string UserName { get; set; }
        public int TimekeeperIndex { get; set; }
        public decimal WorkHrs { get; set; }
        public DateTime WorkDate { get; set; }
        public decimal WeeklyHoursRequired { get; set; }
        public string Workcalendar { get; set; }
        public string EmailAddress { get; set; }
    }
}
