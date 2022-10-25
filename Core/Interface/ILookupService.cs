using Core.Model.Dashboard.Role;

namespace Core.Interface
{
    public interface ILookupService
    {
        Task<IEnumerable<Role>> GetRoles();
    }
}
