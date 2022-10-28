namespace Core.Model.Dashboard.Process
{
    public class Process
    {
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
        public string ProcessCaption { get; set; }
        public List<User.User> Users { get; set; }
    }
}
