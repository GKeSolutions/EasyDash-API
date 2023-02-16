namespace Core.Model.Dashboard.Process
{
    public class DashProcess
    {
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
        public string ProcessCaption { get; set; }
        public Guid ProcessItemId { get; set; }
        public string UserName { get; set; }
        public string LastUpdated { get; set; }
        public List<User.DashUser> Users { get; set; }
    }
}
