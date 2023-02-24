namespace Core.Model.Reassign
{
    public class ReassignModel
    {
        public string ProcessCode { get; set; }
        public Guid ProcItemId { get; set; }
        public Guid ReassignToUserId { get; set; }
    }
}
