using Core.Model.Dashboard.Role;
using Core.Model.Dashboard.User;

namespace Core.Interface
{
    public interface ILookupService
    {
        Task<IEnumerable<Role>> GetRoles();
        Task<IEnumerable<UsersRole>> GetUsersRoles();
    }
}
