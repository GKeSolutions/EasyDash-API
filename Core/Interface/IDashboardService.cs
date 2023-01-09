using Core.Model.Dashboard.Process;
using Core.Model.Dashboard.User;

namespace Core.Interface
{
    public interface IDashboardService
    {
        Task<IEnumerable<DashUser>> GetUsers();
        Task<IEnumerable<DashProcess>> GetProcesses();
    }
}
