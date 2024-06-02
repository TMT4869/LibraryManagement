using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FA.LibraryManagement.Web.Areas.Member.Controllers
{

    [Area("Member")]
    [Route("Book")]
    public class BookController : Controller
    {
        private UserManager<User> _userManager;
        private const string BASE_API_URL = "http://localhost:5055/";

        public BookController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }


        [Route("Detail")]
        public IActionResult Detail(int bookId)
        {
            var book = GetBookDetailsAsync(bookId).Result;
            var user = _userManager.GetUserAsync(User).Result;
            bool IsExistCart = false;
            if (user != null)
            {
                int userId = user.Id;
                var cart = new CartVM()
                {
                    BookId = bookId,
                    UserId = userId,
                };
                CartVM cartVM = GetBookAndUserInCart(cart).Result;
                if (cartVM != null)
                {
                    IsExistCart = true;
                }
                book.IsExistCart = IsExistCart;
            }
            return View(book);
        }

        public IActionResult History()
        {
            return View();
        }


        [HttpPost]
        [Route("Detail")]
        [Authorize]
        public IActionResult Detail(BookVM bookVM)
        {
            var user = _userManager.GetUserAsync(User).Result;
            int userId = user.Id;

            CartVM cartVM = new CartVM
            {
                BookId = bookVM.Id,
                UserId = userId
            };

            int result = AddBookToCartAsync(cartVM).Result;

            if (result > 0)
            {
                TempData["Message"] = "Cart updated successfully";
                return RedirectToAction("Index", "Home", new { area = "Member" });
            }
            return View(bookVM);
        }

        private async Task<int> AddBookToCartAsync(CartVM cartVM)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(BASE_API_URL);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage Res = await client.PostAsJsonAsync("api/Cart/add-book-to-cart", cartVM);
            var apiResponse = Res.Content.ReadAsStringAsync().Result;
            if (Res.IsSuccessStatusCode)
            {
                return 1;
            }

            return 0;
        }

        private async Task<BookVM> GetBookDetailsAsync(int bookId)
        {
            BookVM bookDetail = null;
            string baseUrl = "http://localhost:5055/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/Book/get-book-by-id/" + bookId);
                if (Res.IsSuccessStatusCode)
                {
                    var bookResponse = await Res.Content.ReadAsStringAsync();
                    bookDetail = JsonConvert.DeserializeObject<BookVM>(bookResponse);

                    if (bookDetail.PublishedDate != DateOnly.MinValue)
                    {
                        bookDetail.PublishedDateString = bookDetail.PublishedDate.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        bookDetail.PublishedDateString = string.Empty;
                    }
                    return bookDetail;
                }
                return null;
            }
        }

        private async Task<CartVM> GetBookAndUserInCart(CartVM cartVM)
        {
            CartVM cart = null;
            string baseUrl = "http://localhost:5055/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.PostAsJsonAsync("api/Cart/get-book-and-user-in-cart", cartVM);
                if (Res.IsSuccessStatusCode)
                {
                    var cartResponse = Res.Content.ReadAsStringAsync().Result;
                    cart = JsonConvert.DeserializeObject<CartVM>(cartResponse);

                    return cart;
                }
                return null;
            }
        }
    }
}
