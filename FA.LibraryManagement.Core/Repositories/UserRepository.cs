using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FA.LibraryManagement.Core.Repositories;

/// <summary>
/// The user repository class
/// </summary>
public class UserRepository(LibraryManagementContext context) : BaseRepository<User>(context), IUserRepository
{
    /// <summary>
    /// Adds the user using the specified user
    /// </summary>
    /// <param name="user">The user</param>
    public void AddUser(User user)
    {
        Create(user);
    }

    /// <summary>
    /// Updates the user using the specified user
    /// </summary>
    /// <param name="user">The user</param>
    public void UpdateUser(User user)
    {
        Update(user);
    }

    /// <summary>
    /// Deletes the user using the specified user
    /// </summary>
    /// <param name="user">The user</param>
    public void DeleteUser(User user)
    {
        Delete(user);
    }

    /// <summary>
    /// Deletes the user using the specified user id
    /// </summary>
    /// <param name="userId">The user id</param>
    public void DeleteUser(int userId)
    {
        Delete(userId);
    }

    /// <summary>
    /// Finds the user id
    /// </summary>
    /// <param name="userId">The user id</param>
    /// <returns>The user</returns>
    public User Find(int userId)
    {
        return GetById(userId);
    }

    /// <summary>
    /// Gets the all users
    /// </summary>
    /// <returns>A list of user</returns>
    public IList<User> GetAllUsers()
    {
        return GetAll().ToList();
    }

    /// <summary>
    /// Gets the paged using the specified skip
    /// </summary>
    /// <param name="skip">The skip</param>
    /// <param name="pageSize">The page size</param>
    /// <param name="searchValue">The search value</param>
    /// <param name="sortColumn">The sort column</param>
    /// <param name="sortColumnDirection">The sort column direction</param>
    /// <exception cref="NotImplementedException"></exception>
    /// <returns>A list of category</returns>
    public PagedResult<User> GetPaged(int skip, int pageSize, string? searchValue, string? sortColumn,
        string? sortColumnDirection)
    {
        var query = DbSet
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsEnumerable();

        if (!string.IsNullOrEmpty(searchValue))
            query = query.Where(c => c.Email.Contains(searchValue));

        if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
        {
            query = ApplySorting(query, sortColumn, sortColumnDirection);
        }
        else
        {
            query = query.OrderBy(c => c.Id);
        }

        var totalRecords = query.Count();

        var results = query
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        return new PagedResult<User> { TotalRecords = totalRecords, Results = results };
    }

    public int Count(string role)
    {
        return DbSet.Count(u => u.UserRoles.Any(ur => ur.Role.Name == role));
    }

    public List<User> GetNewMembers()
    {
        return DbSet
            .OrderByDescending(u => u.Id)
            .Take(5)
            .ToList();
    }

    private IOrderedEnumerable<User> ApplySorting(IEnumerable<User> query, string? sortColumn,
        string? sortColumnDirection)
    {
        return (sortColumn switch
        {
            "#" when sortColumnDirection == "asc" => query.OrderBy(c => c.Id),
            "#" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Id),
            "Email" when sortColumnDirection == "asc" => query.OrderBy(c => c.Email),
            "Email" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Email),
            "UserName" when sortColumnDirection == "asc" => query.OrderBy(c => c.UserName),
            "UserName" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.UserName),
            "FullName" when sortColumnDirection == "asc" => query.OrderBy(c => c.FirstName),
            "FullName" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.FirstName),
            "PhoneNumber" when sortColumnDirection == "asc" => query.OrderBy(c => c.PhoneNumber),
            "PhoneNumber" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.PhoneNumber),
            "RoleName" when sortColumnDirection == "asc" => query.OrderBy(c => c.UserRoles.FirstOrDefault()?.Role.Name),
            "RoleName" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.UserRoles.FirstOrDefault()?.Role.Name),
            // Handle other cases for sorting on different columns
            _ => throw new ArgumentException("Invalid sort column.", nameof(sortColumn))
        })!;
    }
}