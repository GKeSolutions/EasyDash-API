using Core.Model;

namespace Core.Interface
{
    public interface ILookupService
    {
        Task<IEnumerable<Role>> GetRoles();
    }
}
