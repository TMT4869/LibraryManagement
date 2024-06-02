using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.Repositories
{
    public class BorrowingRepository : BaseRepository<Borrowing>, IBorrowingRepository
    {
        private readonly LibraryManagementContext _context;
        public BorrowingRepository(LibraryManagementContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateStatus(int id, string status)
        {
            var borrowing = _context.Borrowings.Find(id);
            if (borrowing != null)
            {
                borrowing.Status = status;
                _context.Borrowings.Update(borrowing);
            }
        }

        public int CountByStatus(string status)
        {
            return _context.Borrowings.Count(b => b.Status == status);
        }
    }
}
