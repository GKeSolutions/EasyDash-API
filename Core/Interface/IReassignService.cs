using Core.Model.Dashboard.Role;

namespace Core.Interface
{
    public interface IReassignService
    {
        Task<string> Reassign(string processCode, Guid procItemId, Guid reassignToUserId, string userName);
    }
}
