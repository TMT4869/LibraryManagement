using FA.LibraryManagement.Core.Context;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.IRepositories;
using FA.LibraryManagement.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FA.LibraryManagement.Core.Repositories
{
    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        private readonly LibraryManagementContext _context;

        public BookRepository(LibraryManagementContext context) : base(context)
        {
            _context = context;
        }

        public IList<Book> GetBooksByCategory(string category)
        {
            return _context.Books.Include(b => b.Category).Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Where(b => b.Category.Name == category).ToList();
        }

        public IList<Book> SearchBooks(string keyword)
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors).ThenInclude(ba => ba.Author)
                .Where(b => b.Title.Contains(keyword) || b.BookAuthors.Any(ba => ba.Author.Name.Contains(keyword)))
                .ToList();
        }

        public PagedResult<Book> GetPaged(int skip, string? quantity, int pageSize, string? searchValue,
            string? sortColumn,
            string? sortColumnDirection)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookImages)
                .AsEnumerable();

            if (!string.IsNullOrEmpty(quantity))
            {
                if (quantity.Equals("available"))
                    query = query.Where(b => b.Quantity > 0);
                else if (quantity.Equals("out-of-stock"))
                    query = query.Where(b => b.Quantity == 0);
            }


            if (!string.IsNullOrEmpty(searchValue))
            {
                query = query.Where(b =>
                    b.Title.Contains(searchValue) || b.BookAuthors.Any(ba => ba.Author.Name.Contains(searchValue) ||
                        b.Category.Name.Contains(searchValue) || b.Publisher.Contains(searchValue) ||
                        b.ISBN.Contains(searchValue) || b.PublishedDate.ToString().Contains(searchValue) ||
                        b.Quantity.ToString().Contains(searchValue)));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                query = ApplySorting(query, sortColumn, sortColumnDirection);
            }

            var totalRecords = query.Count();
            var results = query.Skip(skip).Take(pageSize).ToList();

            return new PagedResult<Book>
            {
                Results = results,
                TotalRecords = totalRecords
            };
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookImages)
                .FirstOrDefault(b => b.Id == bookId);
        }

        public IList<Book> GetAllBooksByCategory(int categoryId, string keyword)
        {
            var query = _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookImages)
                .AsQueryable();

            if (categoryId != 0)
            {
                query = query.Where(i => i.CategoryId == categoryId);
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(i => i.BookAuthors.FirstOrDefault().Author.Name.Contains(keyword) || i.Title.Contains(keyword) || i.PublishedDate.ToString().Contains(keyword));
            }


            return query
                .ToList();
        }

        public IList<Book> GetAllBooks(string keyword)
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookImages)
                .Where(i => string.IsNullOrEmpty(keyword) || i.Description.Contains(keyword) || i.BookAuthors.FirstOrDefault().Author.Name.Contains(keyword) || i.Title.Contains(keyword))
                .ToList();
        }

        public void UpdateQuantityBook(int bookId, int quantity)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                book.Quantity = quantity;
            }
        }

        public int GetLastBookId()
        {
            return _context.Books.Max(b => b.Id);
        }

        public int CountByQuantity()
        {
            return _context.Books.Sum(b => b.Quantity);
        }

        public List<Book> GetNewBooks()
        {
            return _context
                .Books
                .Include(b => b.Category)
                .Include(b => b.BookAuthors)
                .ThenInclude(ba => ba.Author)
                .Include(b => b.BookImages)
                .OrderByDescending(b => b.Id)
                .Take(5)
                .ToList();
        }

        private IOrderedEnumerable<Book> ApplySorting(IEnumerable<Book> query, string? sortColumn,
            string? sortColumnDirection)
        {
            return (sortColumn switch
            {
                "#" when sortColumnDirection == "asc" => query.OrderBy(c => c.Id),
                "#" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Id),
                "Title" when sortColumnDirection == "asc" => query.OrderBy(c => c.Title),
                "Title" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Title),
                "ISBN" when sortColumnDirection == "asc" => query.OrderBy(c => c.ISBN),
                "ISBN" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.ISBN),
                "Publisher" when sortColumnDirection == "asc" => query.OrderBy(c => c.Publisher),
                "Publisher" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Publisher),
                "Quantity" when sortColumnDirection == "asc" => query.OrderBy(c => c.Quantity),
                "Quantity" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Quantity),
                "CategoryName" when sortColumnDirection == "asc" => query.OrderBy(c => c.Category.Name),
                "CategoryName" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Category.Name),
                "PublishedDateString" when sortColumnDirection == "asc" => query.OrderBy(c => c.Id),
                "PublishedDateString" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.Id),
                "AuthorName" when sortColumnDirection == "asc" => query.OrderBy(c => c.BookAuthors.First().Author.Name),
                "AuthorName" when sortColumnDirection == "desc" => query.OrderByDescending(c => c.BookAuthors.First().Author.Name),

                // Handle other cases for sorting on different columns
                _ => throw new ArgumentException("Invalid sort column.", nameof(sortColumn))
            })!;
        }
    }
}