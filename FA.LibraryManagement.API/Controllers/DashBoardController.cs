using AutoMapper;
using FA.LibraryManagement.Common.Helper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashBoardController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public DashBoardController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("summary")]
    public IActionResult GetBorrowingDetail()
    {
        var totalBooks = _unitOfWork.BookRepository.CountByQuantity();
        var totalFine = _unitOfWork.BorrowingDetailRepository.TotalFine();
        var totalMembers = _unitOfWork.UserRepository.Count("Member");

        var newMembers = _unitOfWork.UserRepository.GetNewMembers();
        var newBooks = _unitOfWork.BookRepository.GetNewBooks();

        var borrowings = _unitOfWork.BorrowingDetailRepository.GetBorrowingTodayList("Borrowing");
        var borrowingTodayListVM = _mapper.Map<List<BorrowingTodayListVM>>(borrowings);
        var dashboardVM = new DashBoardVM()
        {
            TotalBooks = totalBooks,
            TotalMembers = totalMembers,
            TotalFine = totalFine,
            BorrowingTodayList = borrowingTodayListVM,
            UserList = _mapper.Map<List<UserVM>>(newMembers),
            BookList = _mapper.Map<List<BookVM>>(newBooks)
        };
        return Ok(dashboardVM);
    }

    [HttpGet("books-report")]
    public IActionResult Summary()
    {

        var totalPendingBooks = _unitOfWork.BorrowingDetailRepository.CountByStatus("Pending");
        var totalCancelledBooks = _unitOfWork.BorrowingDetailRepository.CountByStatus("Cancelled");
        var totalBorrowingBooks = _unitOfWork.BorrowingDetailRepository.CountByStatus("Borrowing");
        var totalReturnedBooks = _unitOfWork.BorrowingDetailRepository.CountByStatus("Returned");

        var data = new Dictionary<string, int>
        {
            {Constant.Pending, totalPendingBooks},
            {Constant.Cancelled, totalCancelledBooks},
            {Constant.Borrowing, totalBorrowingBooks},
            {Constant.Returned, totalReturnedBooks}
        };
        return Ok(data);
    }
}