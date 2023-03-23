﻿using Core.Model.Dashboard;
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
        Task<IEnumerable<ProcessResult>> GetProcessItemsByProcessCodeFromDb(string processCode);
        Task<IEnumerable<ProcessResult>> GetOpenProcessesPerTemplate(int templateId);
        Task<ProcessResult> GetProcessInfoByProcId(Guid processId);
        Task<string> CancelProcess(string processCode, Guid procItemId);
        Task<string> CancelProcesses(Cancel model);
    }
}
