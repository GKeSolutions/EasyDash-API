namespace Core.Model.Dashboard.Process
{
    public class DashProcess
    {
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
        public string ProcessCaption { get; set; }
        public List<User.DashUser> Users { get; set; }
    }
}
