using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.IRepositories;

public interface ICategoryRepository : IBaseRepository<Category>
{
    public PagedResult<Category> GetPaged(int skip, int pageSize, string? searchValue, string? sortColumn, string? sortColumnDirection);
}