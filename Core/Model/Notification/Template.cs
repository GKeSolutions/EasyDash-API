namespace Core.Model.Notification
{
    public class Template
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDefault { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int Priority { get; set; }
        public Guid? Role { get; set; }
        public string Process { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateBody { get; set; }
    }
}
