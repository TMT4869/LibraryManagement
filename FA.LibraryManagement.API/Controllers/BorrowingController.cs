using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("add-borrowing")]
        public IActionResult AddBorrowing([FromBody] BorrowingVM borrowingVM)
        {
            var borrowing = _mapper.Map<Borrowing>(borrowingVM);
            _unitOfWork.BorrowingRepository.Create(borrowing);
            var result = _unitOfWork.SaveChanges();
            if (result > 0)
            {
                var createdBorrowingVM = _mapper.Map<BorrowingVM>(borrowing);
                return Ok(createdBorrowingVM);

            }

            return BadRequest(new
            {
                success = false,
                message = "Failed !"
            });
        }

        [HttpGet("get-borrowing-by-user-id/{id}")]
        public IActionResult GetBorrowingByUserId(int id)
        {
            // Get all borrowings of the user and order them by the borrowing time in descending order
            var borrowings = _unitOfWork.BorrowingRepository.GetAll(u => u.UserId == id)
                                                            .OrderByDescending(b => b.Id);
            // If there are no borrowings, return NotFound
            if (!borrowings.Any()) return NotFound();

            // Get the most recent borrowing
            var borrowing = borrowings.First();

            var borrowingVM = _mapper.Map<BorrowingVM>(borrowing);
            var user = _unitOfWork.UserRepository.Get(u => u.Id == borrowingVM.UserId);

            borrowingVM.UserVM = _mapper.Map<UserVM>(user);
            var borrowingDetails = _unitOfWork.BorrowingDetailRepository.GetAll(b => b.BorrowingId == borrowingVM.Id);

            borrowingVM.BorrowingDetailsVM = _mapper.Map<IEnumerable<BorrowingDetailVM>>(borrowingDetails);

            return Ok(borrowingVM);
        }

        [HttpGet("get-all-borrowing-by-user-id/{id}")]
        public IActionResult GetAllBorrowingByUserId(int id, string status = "all")
        {
            var borrowings = _unitOfWork.BorrowingRepository.GetAll(u => u.UserId == id);

            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                borrowings = borrowings.Where(b => b.Status.ToLower() == status);
            }

            if (borrowings == null) return NotFound();

            var borrowingVMs = _mapper.Map<IEnumerable<BorrowingVM>>(borrowings);
            foreach (var borrowingVM in borrowingVMs)
            {
                var user = _unitOfWork.UserRepository.Get(u => u.Id == borrowingVM.UserId);
                borrowingVM.UserVM = _mapper.Map<UserVM>(user);
            }
            var data = borrowingVMs;
            return Ok(new
            {
                data
            });
        }

        [HttpGet("get-borrowing-by-id/{id}")]
        public IActionResult GetBorrowingById(int id)
        {
            var borrowing = _unitOfWork.BorrowingRepository.Get(u => u.Id == id);
            if (borrowing == null) return NotFound();

            var borrowingVM = _mapper.Map<BorrowingVM>(borrowing);
            var user = _unitOfWork.UserRepository.Get(u => u.Id == borrowingVM.UserId);
            borrowingVM.UserVM = _mapper.Map<UserVM>(user);
            var borrowingDetails = _unitOfWork.BorrowingDetailRepository.GetAll(b => b.BorrowingId == borrowingVM.Id);
            borrowingVM.BorrowingDetailsVM = _mapper.Map<IEnumerable<BorrowingDetailVM>>(borrowingDetails);

            // Map BookVM for each BorrowingDetailVM
            foreach (var detail in borrowingVM.BorrowingDetailsVM)
            {
                var book = _unitOfWork.BookRepository.GetBookById(detail.BookId);
                detail.BookVM = _mapper.Map<BookVM>(book);
            }

            return Ok(borrowingVM);
        }

        [HttpGet("get-all-borrowing")]
        public IActionResult GetAllBorrowing(string status = "all")
        {
            var borrowings = _unitOfWork.BorrowingRepository.GetAll();

            if (!string.IsNullOrEmpty(status) && status != "all")
            {
                borrowings = borrowings.Where(b => b.Status.ToLower() == status);
            }

            if (borrowings == null) return NotFound();

            var borrowingVMs = _mapper.Map<IEnumerable<BorrowingVM>>(borrowings);
            foreach (var borrowingVM in borrowingVMs)
            {
                var user = _unitOfWork.UserRepository.Get(u => u.Id == borrowingVM.UserId);
                borrowingVM.UserVM = _mapper.Map<UserVM>(user);
            }
            var data = borrowingVMs;
            return Ok(new
            {
                data
            });
        }

        [HttpPost("add-borrowing-detail")]
        public IActionResult AddBorrowingDetail([FromBody] BorrowingDetailVM borrowingDetailVM)
        {
            var borrowingDetail = _mapper.Map<BorrowingDetail>(borrowingDetailVM);
            _unitOfWork.BorrowingDetailRepository.Create(borrowingDetail);
            var result = _unitOfWork.SaveChanges();
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

        [HttpPut("update-borrowing-status/{id}")]
        public IActionResult UpdateBorrowingStatus(int id, [FromBody] string status)
        {
            _unitOfWork.BorrowingRepository.UpdateStatus(id, status);
            var result = _unitOfWork.SaveChanges();
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

        [HttpPut("update-borrowing-detail-status/{id}")]
        public IActionResult UpdateBorrowingDetailStatus(int id, [FromBody] string status)
        {
            _unitOfWork.BorrowingDetailRepository.UpdateStatus(id, status);
            var result = _unitOfWork.SaveChanges();
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

        [HttpPut("update-borrowing-by-id/{id}")]
        public IActionResult UpdateBorrowingById(int id, [FromBody] BorrowingVM borrowingVM)
        {
            var borrowing = _unitOfWork.BorrowingRepository.Get(u => u.Id == id);
            if (borrowing == null) return NotFound();

            _mapper.Map(borrowingVM, borrowing);
            _unitOfWork.BorrowingRepository.Update(borrowing);
            var result = _unitOfWork.SaveChanges();
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

        [HttpGet("get-borrowing-detail-by-id/{id}")]
        public IActionResult GetBorrowingDetailById(int id)
        {
            var borrowingDetail = _unitOfWork.BorrowingDetailRepository.Get(u => u.Id == id);
            if (borrowingDetail == null) return NotFound();

            var borrowingDetailVM = _mapper.Map<BorrowingDetailVM>(borrowingDetail);
            var book = _unitOfWork.BookRepository.Get(u => u.Id == borrowingDetailVM.BookId);
            borrowingDetailVM.BookVM = _mapper.Map<BookVM>(book);
            return Ok(borrowingDetailVM);
        }

        [HttpPut("update-borrowing-detail-by-id/{id}")]
        public IActionResult UpdateBorrowingDetailById(int id, [FromBody] BorrowingDetailVM borrowingDetailVM)
        {
            var borrowingDetail = _unitOfWork.BorrowingDetailRepository.Get(u => u.Id == id);
            if (borrowingDetail == null) return NotFound();

            _mapper.Map(borrowingDetailVM, borrowingDetail);
            _unitOfWork.BorrowingDetailRepository.Update(borrowingDetail);
            var result = _unitOfWork.SaveChanges();
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
    }
}
