using Core.Model.Dashboard;

namespace Core.Interface
{
    public interface ICancelService
    {
        Task<string> CancelProcess(string processCode, Guid procItemId);
        Task<string> CancelAll(Cancel model);
    }
}
