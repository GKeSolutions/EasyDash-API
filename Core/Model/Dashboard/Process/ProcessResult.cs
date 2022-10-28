namespace Core.Model.Dashboard.Process
{
    public class ProcessResult
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string LastUpdated { get; set; }
        public Guid ProcessItemId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
        public string ProcessCaption { get; set; }
    }
}
