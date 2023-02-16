using Core.Model.Dashboard.Process;
using Core.Model.Dashboard.User;

namespace Core.Interface
{
    public interface IDashboardService
    {
        Task<IEnumerable<DashUser>> GetUsers();
        Task<IEnumerable<DashProcess>> GetProcesses();
        Task<IEnumerable<DashProcess>> GetProcessesByUser(Guid userId);
        Task<IEnumerable<DashUser>> GetProcessItemsByProcessCode(string processCode);
        Task<IEnumerable<ProcessResult>> GetOpenProcessesPerTemplate(int templateId);
        Task<ProcessResult> GetProcessInfoByProcId(string processId);
    }
}
