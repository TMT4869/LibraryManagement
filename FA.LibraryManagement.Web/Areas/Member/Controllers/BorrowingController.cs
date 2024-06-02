using FA.LibraryManagement.Common.Helper;
using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FA.LibraryManagement.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Route("[controller]/[action]")]
    public class BorrowingController : Controller
    {
        string baseUrl = Constant.BASE_API_URL;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int borrowingId)
        {
            var borrowingVM = GetBorrowingByIdAsync(borrowingId).Result;
            return View(borrowingVM);
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var borrowingVM = await GetBorrowingByIdAsync(id);
            if (borrowingVM == null)
            {
                return RedirectToAction(nameof(Detail), new { borrowingId = id });
            }

            borrowingVM.Status = "Cancelled";
            await UpdateBorrowingById(id, borrowingVM);

            foreach (var borrowingDetail in borrowingVM.BorrowingDetailsVM)
            {
                await UpdateBorrowingDetailStatus(borrowingDetail.Id, "Cancelled");
            }

            return RedirectToAction(nameof(Detail), new { borrowingId = id });
        }

        private async Task<BorrowingVM> GetBorrowingByIdAsync(int id)
        {
            BorrowingVM borrowingVM = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/Borrowing/get-borrowing-by-id/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var borrowingResponse = Res.Content.ReadAsStringAsync().Result;
                    borrowingVM = JsonConvert.DeserializeObject<BorrowingVM>(borrowingResponse);
                }
            }
            return borrowingVM;
        }

        #region API Calls
        private async Task<HttpResponseMessage> UpdateBorrowingDetailStatus(int id, string status)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(status), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"api/Borrowing/update-borrowing-detail-status/{id}", content);

            return response;
        }

        private async Task<BorrowingVM> UpdateBorrowingById(int id, BorrowingVM borrowingVM)
        {
            BorrowingVM borrowing = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(JsonConvert.SerializeObject(borrowingVM), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await httpClient.PutAsync("api/Borrowing/update-borrowing-by-id/" + id, content);
                if (Res.IsSuccessStatusCode)
                {
                    var borrowingResponse = Res.Content.ReadAsStringAsync().Result;
                    borrowing = JsonConvert.DeserializeObject<BorrowingVM>(borrowingResponse);
                }
            }
            return borrowing;
        }
        #endregion
    }
}
