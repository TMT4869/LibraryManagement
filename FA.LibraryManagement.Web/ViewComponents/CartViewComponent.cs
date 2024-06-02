using FA.LibraryManagement.Common.Helper;
using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace FA.LibraryManagement.Web.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        string baseUrl = Constant.BASE_API_URL;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (userId != null)
            {
                var cartList = new CartListVM();
                cartList.Carts = await GetCartsByUserIdAsync(int.Parse(userId));
                HttpContext.Session.SetInt32("SessionCart", cartList.Carts.Count());

                return View(HttpContext.Session.GetInt32("SessionCart"));
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
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
