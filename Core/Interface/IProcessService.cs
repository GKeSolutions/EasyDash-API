using Core.Model;

namespace Core.Interface
{
    public interface IProcessService
    {
        Task<object> GetProcesses(int orderBy);
    }
}
