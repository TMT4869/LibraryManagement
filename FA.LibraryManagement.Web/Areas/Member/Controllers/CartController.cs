using FA.LibraryManagement.Common.Helper;
using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace FA.LibraryManagement.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Route("[controller]/[action]")]
    public class CartController : Controller
    {
        string baseUrl = Constant.BASE_API_URL;
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartList = new CartListVM();
            cartList.Carts = await GetCartsByUserIdAsync(int.Parse(userId));

            var borrowing = await GetBorrowingByUserIdAsync(int.Parse(userId));
            if (borrowing != null && borrowing.Status != "Completed" && borrowing.Status != "Cancelled")
            {
                ViewBag.Message = "Please return the book or cancel the current borrowing before borrowing another.";
            }

            return View(cartList);
        }

        public IActionResult Remove(int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = httpClient.DeleteAsync("api/Cart/delete-cart/" + id).Result;
                if (Res.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> SummaryAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartList = new CartListVM();
            cartList.Carts = await GetCartsByUserIdAsync(int.Parse(userId));

            return View(cartList);
        }

        [HttpPost]
        [ActionName("Summary")]
        public async Task<IActionResult> SummaryPOSTAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var cartList = new CartListVM();
            cartList.Carts = await GetCartsByUserIdAsync(int.Parse(userId));

            // Create a new Borrowing
            var borrowingVM = new BorrowingVM
            {
                UserId = int.Parse(userId),
                BorrowedTime = DateOnly.FromDateTime(DateTime.Now),
                Status = "Pending"
            };

            var createdBorrowing = await CreateBorrowingAsync(borrowingVM);

            // Create BorrowingDetail for each Cart
            foreach (var cart in cartList.Carts)
            {
                var borrowingDetailVM = new BorrowingDetailVM
                {
                    BorrowingId = createdBorrowing.Id,
                    BookId = cart.BookVM.Id,
                    DueTime = DateOnly.FromDateTime(DateTime.Now.AddDays(14)),
                    ReturnTime = DateOnly.MinValue, // Not returned yet
                    Status = "Pending",
                    Fine = 0
                };

                var createdBorrowingDetail = await CreateBorrowingDetailAsync(borrowingDetailVM);
            }

            // Delete all carts for the user
            await DeleteCartsByUserIdAsync(int.Parse(userId));

            return RedirectToAction(nameof(BorrowingConfirmation));
        }

        public IActionResult BorrowingConfirmation()
        {
            return View();
        }

        private async Task<BorrowingVM> GetBorrowingByUserIdAsync(int userId)
        {
            BorrowingVM borrowingVM = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/Borrowing/get-borrowing-by-user-id/" + userId);
                if (Res.IsSuccessStatusCode)
                {
                    var borrowingResponse = Res.Content.ReadAsStringAsync().Result;
                    borrowingVM = JsonConvert.DeserializeObject<BorrowingVM>(borrowingResponse);
                }
            }
            return borrowingVM;
        }

        private async Task<BorrowingVM> CreateBorrowingAsync(BorrowingVM borrowingVM)
        {
            BorrowingVM createdBorrowing = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(borrowingVM);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("api/Borrowing/add-borrowing", data);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    createdBorrowing = JsonConvert.DeserializeObject<BorrowingVM>(result);
                }
            }

            return createdBorrowing;
        }

        private async Task<BorrowingDetailVM> CreateBorrowingDetailAsync(BorrowingDetailVM borrowingDetailVM)
        {
            BorrowingDetailVM createdBorrowingDetail = null;

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var json = JsonConvert.SerializeObject(borrowingDetailVM);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("api/Borrowing/add-borrowing-detail", data);
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsStringAsync().Result;
                    createdBorrowingDetail = JsonConvert.DeserializeObject<BorrowingDetailVM>(result);
                }
            }

            return createdBorrowingDetail;
        }

        private async Task DeleteCartsByUserIdAsync(int userId)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                await httpClient.DeleteAsync("api/Cart/delete-all-carts-by-user-id/" + userId);
            }
        }

        private async Task<UserVM> GetUserByIdAsync(int userId)
        {
            UserVM userVM = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/User/get-user-by-id/" + userId);
                if (Res.IsSuccessStatusCode)
                {
                    var userResponse = Res.Content.ReadAsStringAsync().Result;
                    userVM = JsonConvert.DeserializeObject<UserVM>(userResponse);
                }
            }
            return userVM;
        }

        private async Task<List<CartVM>> GetCartsByUserIdAsync(int userId)
        {
            List<CartVM> cartVMs = new List<CartVM>();
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/Cart/get-all-carts-by-user-id/" + userId);
                if (Res.IsSuccessStatusCode)
                {
                    var cartResponse = Res.Content.ReadAsStringAsync().Result;
                    cartVMs = JsonConvert.DeserializeObject<List<CartVM>>(cartResponse);
                }
            }
            return cartVMs;
        }
    }
}
