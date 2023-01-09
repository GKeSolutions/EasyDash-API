using Core.Model.Dashboard.Process;

namespace Core.Model.Dashboard.User
{
    public class DashUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public List<ProcessItem> Processes { get; set; }
        public string LastUpdated { get; set; }
        public List<Role.Role> Roles { get; set; } = new List<Role.Role>();

    }
}
