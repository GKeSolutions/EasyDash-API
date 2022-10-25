using Core.Model.Dashboard.Process;
using Core.Model.Dashboard.User;

namespace Core.Interface
{
    public interface IDashboardService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<Process>> GetProcesses();
    }
}
