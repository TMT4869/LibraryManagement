using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.IRepositories
{
    public interface IHistoryRepository : IBaseRepository<History>
    {
        History GetReturnHistoryByUserId(int userId);
    }
}
