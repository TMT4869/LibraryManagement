using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CartController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("get-book-and-user-in-cart")]
        public IActionResult GetBookAndUserInCart(CartVM cartVM)
        {
            var cart = _unitOfWork.CartRepository.Get(c => c.UserId == cartVM.UserId && c.BookId == cartVM.BookId);
            if (cart == null)
            {
                return NotFound();
            }
            return Ok(cart);
        }

        [HttpGet("get-all-carts-by-user-id/{id}")]
        public IActionResult GetAllCarts(int id)
        {
            var carts = _unitOfWork.CartRepository.GetAll(u => u.UserId == id);
            if (carts == null) return NotFound();

            var cartsVM = _mapper.Map<List<CartVM>>(carts);
            foreach (var cartVM in cartsVM)
            {
                var book = _unitOfWork.BookRepository.GetBookById(cartVM.BookId);
                cartVM.BookVM = _mapper.Map<BookVM>(book);
            }
            return Ok(cartsVM);
        }

        [HttpPost("add-book-to-cart")]
        public IActionResult AddBookToCart([FromBody] CartVM cartVM)
        {
            var cart = _mapper.Map<Cart>(cartVM);
            _unitOfWork.CartRepository.Create(cart);
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

        [HttpDelete("delete-cart/{id}")]
        public IActionResult DeleteCart(int id)
        {
            var cart = _unitOfWork.CartRepository.Get(c => c.Id == id);
            if (cart == null) return NotFound();

            _unitOfWork.CartRepository.Delete(cart);
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

        [HttpDelete("delete-all-carts-by-user-id/{userId}")]
        public IActionResult DeleteAllCartsByUserId(int userId)
        {
            var carts = _unitOfWork.CartRepository.GetAll(c => c.UserId == userId);
            foreach (var cart in carts)
            {
                _unitOfWork.CartRepository.Delete(cart);
            }
            var result = _unitOfWork.SaveChanges();
            if (result > 0)
            {
                return Ok(new
                {
                    success = true,
                    message = "All carts deleted successfully!"
                });
            }

            return BadRequest(new
            {
                success = false,
                message = "Failed to delete carts!"
            });
        }
    }
}
