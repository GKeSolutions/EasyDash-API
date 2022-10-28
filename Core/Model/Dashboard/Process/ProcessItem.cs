namespace Core.Model.Dashboard.Process
{
    public class ProcessItem
    {
        public string LastUpdated { get; set; }
        public Guid ProcessItemId { get; set; }
        public string ProcessCode { get; set; }
        public string ProcessDescription { get; set; }
        public string ProcessCaption { get; set; }
    }
}
