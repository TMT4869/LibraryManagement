using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.Repositories;

public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
{
    private readonly LibraryManagementContext _context;

    public UserRoleRepository(LibraryManagementContext context) : base(context)
    {
        _context = context;
    }

    public void Update(User entity, Role role)
    {
        var userRole = _context.UserRoles.FirstOrDefault(x => x.UserId == entity.Id);
        if (userRole != null)
        {
            // Delete the existing UserRole entity
            _context.UserRoles.Remove(userRole);
            _context.SaveChanges();
        }

        // Create a new UserRole entity with the updated RoleId
        _context.UserRoles.Add(new UserRole
        {
            UserId = entity.Id,
            RoleId = role.Id
        });
    }
}