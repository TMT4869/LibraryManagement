using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.IRepositories
{
    public interface IBorrowingRepository : IBaseRepository<Borrowing>
    {
        void UpdateStatus(int id, string status);
        /*int CountByStatus();*/
        int CountByStatus(string status);
    }
}
