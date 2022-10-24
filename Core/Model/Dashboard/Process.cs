namespace Core.Model.Dashboard
{
    public class Users
    {
        public List<User> User { get; set; }
    }

    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public List<ProcessItem> Processes { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();
    }

    public class ProcessItem
    {
        public string LastUpdated { get; set; }
        public Guid ProcessItemId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
    }

    public class Process
    {
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
        public List<User> Users { get; set; }
    }

    public class ProcessResult
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string LastUpdated { get; set; }
        public Guid ProcessItemId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
    }
}
