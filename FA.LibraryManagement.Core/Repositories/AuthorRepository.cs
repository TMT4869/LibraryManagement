using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.Repositories;

public class AuthorRepository : BaseRepository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryManagementContext context) : base(context)
    {
    }

    public PagedResult<Author> GetPaged(int skip, int pageSize, string? searchValue, string? sortColumn, string? sortColumnDirection)
    {
        var query = GetAll();

        if (!string.IsNullOrEmpty(searchValue)) query = query.Where(c => c.Name.Contains(searchValue));

        if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
        {
            query = ApplySorting(query, sortColumn, sortColumnDirection);
        }

        var totalRecords = query.Count();

        var results = query
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        return new PagedResult<Author> { TotalRecords = totalRecords, Results = results };
    }

    private IOrderedEnumerable<Author> ApplySorting(IEnumerable<Author> query, string? sortColumn,
        string? sortColumnDirection)
    {
        return (sortColumn switch
        {
            "#" when sortColumnDirection == "asc" => query.OrderBy(c => c.Id),
            "#" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Id),
            "Name" when sortColumnDirection == "asc" => query.OrderBy(c => c.Name),
            "Name" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Name),
            // Handle other cases for sorting on different columns
            _ => throw new ArgumentException("Invalid sort column.", nameof(sortColumn))
        })!;
    }
}