using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FA.LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BookController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-last-book-id")]
        public IActionResult GetLastBookId()
        {
            var lastBook = _unitOfWork.BookRepository.GetLastBookId();
            if (lastBook != null)
                return Ok(lastBook);

            return NotFound();
        }

        [HttpPost("add-book")]
        public IActionResult CreateBook([FromBody] BookVM bookVM)
        {
            var book = _mapper.Map<Book>(bookVM);
            _unitOfWork.BookRepository.Create(book);
            var result = _unitOfWork.SaveChanges();
            if (result > 0)
                return Ok();

            return BadRequest();
        }

        [HttpGet("get-all-books")]
        public IActionResult GetAllBooks(int page = 1, string keyword = "")
        {
            int pageSize = 6;
            var books = _unitOfWork.BookRepository.GetAllBooks(keyword);
            if (books == null) return NotFound();

            var booksVM = _mapper.Map<List<BookVM>>(books);

            var pagedList = booksVM.ToPagedList(page, pageSize);

            return Ok(new
            {
                TotalCount = pagedList.TotalItemCount,
                PageNumber = pagedList.PageNumber,
                PageSize = pagedList.PageSize,
                Items = pagedList.ToList()
            });
        }

        [HttpGet("get-book-by-id/{id}")]
        public IActionResult GetBook(int id)
        {
            var book = _unitOfWork.BookRepository.GetBookById(id);
            if (book != null)
            {
                var bookVM = _mapper.Map<BookVM>(book);
                return Ok(bookVM);
            }

            return NotFound();
        }

        [HttpPut("update-book-by-id/{id}")]
        public IActionResult UpdateBook(int id, [FromBody] BookVM bookVM)
        {
            var book = _unitOfWork.BookRepository.GetBookById(id);
            if (book != null)
            {
                // Remove existing BookAuthors
                foreach (var bookAuthor in book.BookAuthors.ToList())
                {
                    _unitOfWork.BookAuthorRepository.Delete(bookAuthor);
                }

                _mapper.Map(bookVM, book);
                _unitOfWork.BookRepository.Update(book);
                var result = _unitOfWork.SaveChanges();
                if (result > 0)
                    return Ok(new
                    {
                        success = true,
                        message = "Success !"
                    });

                return NotFound(new
                {
                    success = false,
                    message = "Failed !"
                });
            }

            return NotFound();
        }

        [HttpDelete("delete-book-by-id/{id}")]
        public IActionResult DeleteBook(int id)
        {
            int result = 0;
            var book = _unitOfWork.BookRepository.GetById(id);
            if (book != null)
            {
                _unitOfWork.BookRepository.Delete(book);
                result = _unitOfWork.SaveChanges();
            }

            if (result > 0)
                return Ok(new
                {
                    success = true,
                    message = "Success !"
                });

            return BadRequest(new
            {
                success = false,
                message = "Failed !"
            });
        }

        [HttpGet("search-books")]
        public IActionResult SearchBooks(string keyword)
        {
            var searchResults = _unitOfWork.BookRepository.SearchBooks(keyword);
            var booksVM = _mapper.Map<List<BookVM>>(searchResults);
            return Ok(booksVM);
        }

        [HttpPost("get-all-books-by-paging")]
        public IActionResult GetAllBooksByPaging()
        {
            var quantity = Request.Form["quantity"].FirstOrDefault();
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"]
                .FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();

            var pageSize = length != null ? Convert.ToInt32(length) : 0;
            var skip = start != null ? Convert.ToInt32(start) : 0;
            var totalRecords = 0;

            var books = _unitOfWork.BookRepository.GetPaged(skip, quantity, pageSize, searchValue, sortColumn,
                sortColumnDirection);

            var data = books
                .Results
                .Select((book, index) => new BookVM()
                {
                    Id = book.Id,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Publisher = book.Publisher,
                    Quantity = book.Quantity,
                    CategoryName = book.Category.Name,
                    PublishedDateString = book.PublishedDate.ToString("dd/MM/yyyy"),
                    AuthorName = string.Join(", ", book.BookAuthors.Select(ba => ba.Author.Name)),
                    Number = index + 1 + skip
                });

            return Ok(new
            {
                draw,
                recordsFiltered = books.TotalRecords,
                recordsTotal = books.TotalRecords,
                data
            });
        }

        [HttpPut("update-book-quantity/{id}")]
        public IActionResult UpdateBookQuantity(int id, [FromBody] int quantity)
        {
            var book = _unitOfWork.BookRepository.GetBookById(id);
            if (book != null)
            {
                book.Quantity = quantity;
                _unitOfWork.BookRepository.Update(book);
                var result = _unitOfWork.SaveChanges();
                if (result > 0)
                    return Ok(new
                    {
                        success = true,
                        message = "Success !"
                    });

                return NotFound(new
                {
                    success = false,
                    message = "Failed !"
                });
            }

            return NotFound();
        }

        #region Remote Validation

        [HttpGet("check-isbn-in-use/{isbn}")]
        public IActionResult CheckIsbnInUse(string isbn)
        {
            var book = _unitOfWork.BookRepository.Get(b => b.ISBN == isbn);
            if (book == null)
                return Ok(true);
            return Ok($"ISBN {isbn} is already in use");
        }



        #endregion
    }
}