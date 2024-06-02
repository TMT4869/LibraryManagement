using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FA.LibraryManagement.Core.Repositories
{
    public class BorrowingDetailRepository : BaseRepository<BorrowingDetail>, IBorrowingDetailRepository
    {
        private readonly LibraryManagementContext _context;
        public BorrowingDetailRepository(LibraryManagementContext context) : base(context)
        {
            _context = context;
        }

        public void UpdateStatus(int id, string status)
        {
            var borrowingDetail = _context.BorrowingDetails.Find(id);
            if (borrowingDetail != null)
            {
                borrowingDetail.Status = status;
                _context.BorrowingDetails.Update(borrowingDetail);
            }
        }

        public int CountByStatus(string status)
        {
            return _context.BorrowingDetails.Count(b => b.Status == status);
        }

        public float TotalFine()
        {
            return _context.BorrowingDetails.Sum(b => b.Fine);
        }

        public IList<BorrowingDetail> GetBorrowingTodayList(string status)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            var result = _context.BorrowingDetails
                .Include(b => b.Borrowing)
                .ThenInclude(b => b.User)
                .Where(b => b.Status == status && b.Borrowing.BorrowedTime == today)
                .GroupBy(b => new { b.Borrowing.User.UserName, b.Borrowing.BorrowedTime, b.DueTime, b.ReturnTime, b.Fine })
                .Select(b => new BorrowingDetail()
                {
                    Gender = b.FirstOrDefault().Borrowing.User.Gender,
                    ImageUrl = b.FirstOrDefault().Borrowing.User.ImageUrl,
                    UserName = b.FirstOrDefault().Borrowing.User.UserName,
                    BorrowingId = b.FirstOrDefault().BorrowingId,
                    ReturnTime = b.FirstOrDefault().ReturnTime,
                    BorrowedTime = b.FirstOrDefault().Borrowing.BorrowedTime,
                    DueTime = b.FirstOrDefault().DueTime,
                    Fine = b.FirstOrDefault().Fine,
                    NumberOfBooks = b.Count()
                })
                .Take(5)
                .ToList();
            return result;
        }
    }
}
