using Core.Model.Reassign;

namespace Core.Interface
{
    public interface IReassignService
    {
        Task<string> Reassign(string processCode, Guid procItemId, Guid reassignToUserId);
        Task<string> ReassignAll(ReassignModel model);
    }
}
