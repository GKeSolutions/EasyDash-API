using Core.Model.Dashboard;

namespace Core.Interface
{
    public interface ILookupService
    {
        Task<IEnumerable<Role>> GetRoles();
    }
}
