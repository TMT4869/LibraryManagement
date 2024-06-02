using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.IRepositories;

public interface IUserRoleRepository : IBaseRepository<UserRole>
{
    void Update(User entity, Role role);
}