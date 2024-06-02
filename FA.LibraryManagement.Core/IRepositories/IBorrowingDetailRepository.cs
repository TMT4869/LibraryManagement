using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.IRepositories
{
    public interface IBorrowingDetailRepository : IBaseRepository<BorrowingDetail>
    {
        void UpdateStatus(int id, string status);
        int CountByStatus(string status);
        float TotalFine();
        IList<BorrowingDetail> GetBorrowingTodayList(string status);
    }
}
