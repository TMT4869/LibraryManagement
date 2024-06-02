using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Core.IRepositories
{
    public interface IBookRepository : IBaseRepository<Book>
    {
        IList<Book> GetBooksByCategory(string category);
        IList<Book> SearchBooks(string keyword);
        PagedResult<Book> GetPaged(int skip, string? quantity, int pageSize, string? searchValue, string? sortColumn,
            string? sortColumnDirection);

        Book GetBookById(int bookId);

        IList<Book> GetAllBooksByCategory(int categoryId, string keyword);

        IList<Book> GetAllBooks(string keyword);

        void UpdateQuantityBook(int bookId, int quantity);
        int GetLastBookId();
        int CountByQuantity();
        List<Book> GetNewBooks();
    }
}
