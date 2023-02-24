namespace Core.Model.Dashboard.User
{
    public class UserRole
    {
        public Guid NxBaseUserId { get; set; }
        public string BaseUserName { get; set; }
        public bool IsActive { get; set; }
        public string ArchetypeCode { get; set; }
    }
}
