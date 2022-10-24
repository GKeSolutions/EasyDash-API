using Core.Model;

namespace Core.Interface
{
    public interface IDashboardService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<IEnumerable<Process>> GetProcesses();
    }
}
