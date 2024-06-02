using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.Repositories
{
    public class HistoryRepository : BaseRepository<History>, IHistoryRepository
    {
        private readonly LibraryManagementContext _context;
        public HistoryRepository(LibraryManagementContext context) : base(context)
        {
            _context = context;
        }

        public History GetReturnHistoryByUserId(int userId)
        {
            return _context.Histories
                           .Where(h => h.UserId == userId &&
                                       h.Status == "Borrowing")
                           .OrderBy(h => h.DueTime)
                           .FirstOrDefault();
        }
    }
}
