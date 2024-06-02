using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HistoryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("get-all-histories-by-user-id/{id}")]
        public IActionResult GetAllHistoriesByUserId(int id)
        {
            var histories = _unitOfWork.HistoryRepository.GetAll(h => h.UserId == id);
            if (histories == null) return NotFound();

            var historiesVM = _mapper.Map<List<HistoryVM>>(histories);
            foreach (var historyVM in historiesVM)
            {
                var book = _unitOfWork.BookRepository.GetBookById(historyVM.BookId);
                historyVM.BookVM = _mapper.Map<BookVM>(book);
            }

            return Ok(historiesVM);
        }

        [HttpGet("get-history-by-user-id/{userId}")]
        public IActionResult GetAllHistoriesByUserId(int userId, string status = "all")
        {
            var histories = _unitOfWork.HistoryRepository.GetAll(h => h.UserId == userId);
            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                histories = histories.Where(b => b.Status.ToLower() == status);
            }

            if (histories == null) return NotFound();

            var historiesVM = _mapper.Map<IEnumerable<HistoryVM>>(histories);
            foreach (var historyVM in historiesVM)
            {
                var book = _unitOfWork.BookRepository.GetBookById(historyVM.BookId);
                historyVM.BookVM = _mapper.Map<BookVM>(book);
            }

            var data = historiesVM;

            return Ok(new
            {
                data
            });
        }



        [HttpGet("get-history-by-user-id/{userId}/book-id/{bookId}/borrowed-time/{borrowedTime}")]
        public IActionResult GetHistoryByUserIdAndBookIdAndBorrowedTime(int userId, int bookId, string borrowedTime)
        {
            var history = _unitOfWork.HistoryRepository
                .Get(h => h.UserId == userId && h.BookId == bookId && h.BorrowedTime.ToString() == borrowedTime);
            if (history == null) return NotFound();

            var historyVM = _mapper.Map<HistoryVM>(history);
            var book = _unitOfWork.BookRepository.GetBookById(historyVM.BookId);
            historyVM.BookVM = _mapper.Map<BookVM>(book);

            return Ok(historyVM);
        }

        [HttpPost("add-history")]
        public IActionResult AddHistory([FromBody] HistoryVM historyVM)
        {
            var history = _mapper.Map<History>(historyVM);
            _unitOfWork.HistoryRepository.Create(history);
            var result = _unitOfWork.SaveChanges();
            if (result > 0)
                return Ok(historyVM);

            return BadRequest(new
            {
                success = false,
                message = "Failed !"
            });
        }

        [HttpPut("update-history")]
        public IActionResult UpdateHistory([FromBody] HistoryVM historyVM)
        {
            var history = _mapper.Map<History>(historyVM);
            _unitOfWork.HistoryRepository.Update(history);
            var result = _unitOfWork.SaveChanges();
            if (result > 0)
                return Ok(historyVM);

            return BadRequest(new
            {
                success = false,
                message = "Failed !"
            });
        }

        [HttpGet("get-return-history-by-user-id/{id}")]
        public IActionResult GetReturnHistoryByUserId(int id)
        {
            var history = _unitOfWork.HistoryRepository.GetReturnHistoryByUserId(id);
            if (history == null) return NotFound();

            var historyVM = _mapper.Map<HistoryVM>(history);

            return Ok(historyVM);
        }
    }
}
