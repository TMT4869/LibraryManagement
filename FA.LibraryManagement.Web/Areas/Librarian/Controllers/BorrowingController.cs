using FA.LibraryManagement.Common.Helper;
using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace FA.LibraryManagement.Web.Areas.Librarian.Controllers
{
    [Area("Librarian")]
    [Route("Librarian/Borrowing")]
    public class BorrowingController : Controller
    {
        string baseUrl = Constant.BASE_API_URL;

        [Route("List")]
        public IActionResult List()
        {
            return View();
        }

        [Route("Detail")]
        public IActionResult Detail(int borrowingId)
        {
            var borrowingVM = GetBorrowingByIdAsync(borrowingId).Result;
            return View(borrowingVM);
        }

        [HttpPost]
        [Route("Completed")]
        public async Task<IActionResult> Completed(int id)
        {
            var borrowingVM = await GetBorrowingByIdAsync(id);
            if (borrowingVM == null)
            {
                return RedirectToAction(nameof(Detail), new { borrowingId = id });
            }

            borrowingVM.Status = "Completed";

            await UpdateBorrowingById(id, borrowingVM);

            foreach (var borrowingDetail in borrowingVM.BorrowingDetailsVM)
            {
                borrowingDetail.Status = "Returned";
                borrowingDetail.Fine = CalculateFine(borrowingDetail.DueTime, borrowingDetail.ReturnTime);
                borrowingDetail.BookVM.Quantity += 1;

                // Update history
                var historyVM = await GetHistoryByUserIdAndBookIdAndBorrowedTime(borrowingVM.UserId,
                                   borrowingDetail.BookId, borrowingVM.BorrowedTime.ToString("yyyy-MM-dd"));

                historyVM.Fine = borrowingDetail.Fine;
                historyVM.Status = "Borrowed";
                await UpdateHistory(historyVM);

                await UpdateBookQuantity(borrowingDetail.BookVM.Id, borrowingDetail.BookVM.Quantity);
                await UpdateBorrowingDetailById(borrowingDetail.Id, borrowingDetail);
            }

            return RedirectToAction(nameof(Detail), new { borrowingId = id });
        }

        [HttpPost]
        [Route("Cancel")]
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

        [HttpPost]
        [Route("Incomplete")]
        public async Task<IActionResult> Incomplete(int id)
        {
            var borrowingVM = await GetBorrowingByIdAsync(id);
            if (borrowingVM == null)
            {
                return RedirectToAction(nameof(Detail), new { borrowingId = id });
            }

            borrowingVM.BorrowedTime = DateOnly.FromDateTime(DateTime.Now);
            borrowingVM.Status = "Incomplete";

            await UpdateBorrowingById(id, borrowingVM);

            foreach (var borrowingDetail in borrowingVM.BorrowingDetailsVM)
            {
                await UpdateBorrowingDetailStatus(borrowingDetail.Id, "Borrowing");
                borrowingDetail.BookVM.Quantity -= 1;
                await UpdateBookQuantity(borrowingDetail.BookVM.Id, borrowingDetail.BookVM.Quantity);

                // Create history
                var historyVM = new HistoryVM();
                historyVM.UserId = borrowingVM.UserId;
                historyVM.BookId = borrowingDetail.BookVM.Id;
                historyVM.BorrowedTime = borrowingVM.BorrowedTime;
                historyVM.DueTime = borrowingDetail.DueTime;
                historyVM.ReturnedTime = borrowingDetail.ReturnTime;
                historyVM.Fine = borrowingDetail.Fine;
                historyVM.Status = "Borrowing";

                await AddHistory(historyVM);
            }

            return RedirectToAction(nameof(Detail), new { borrowingId = id });
        }

        [HttpPost]
        [Route("BorrowingDetail/{id}")]
        public async Task<IActionResult> BorrowingDetailPartial(int id)
        {
            var borrowingDetailVM = await GetBorrowingDetailByIdAsync(id);
            return PartialView("_BorrowingDetailPartial", borrowingDetailVM);
        }

        [HttpPost]
        [Route("BorrowingDetail/Update")]
        public async Task<IActionResult> UpdateBorrowingDetail(BorrowingDetailVM borrowingDetailVM)
        {
            // Only calculate fine if status is "Returned"
            if (borrowingDetailVM.Status == "Returned")
            {
                borrowingDetailVM.Fine = CalculateFine(borrowingDetailVM.DueTime, borrowingDetailVM.ReturnTime);
            }
            else
            {
                borrowingDetailVM.Fine = 0;
            }

            var response = await UpdateBorrowingDetailById(borrowingDetailVM.Id, borrowingDetailVM);
            if (response != null)
            {
                var borrowingVM = await GetBorrowingByIdAsync(borrowingDetailVM.BorrowingId);
                if (borrowingVM != null)
                {
                    // Update history
                    var historyVM = await GetHistoryByUserIdAndBookIdAndBorrowedTime(borrowingVM.UserId,
                                       borrowingDetailVM.BookId, borrowingVM.BorrowedTime.ToString("yyyy-MM-dd"));

                    historyVM.DueTime = borrowingDetailVM.DueTime;
                    historyVM.ReturnedTime = borrowingDetailVM.ReturnTime;
                    historyVM.Fine = borrowingDetailVM.Fine;
                    if (borrowingDetailVM.Status == "Returned")
                    {
                        historyVM.Status = "Borrowed";
                    }
                    else
                    {
                        historyVM.Status = "Borrowing";
                    }
                    await UpdateHistory(historyVM);
                }

                return RedirectToAction(nameof(Detail), new { borrowingId = borrowingDetailVM.BorrowingId });
            }

            return RedirectToAction(nameof(Detail), new { borrowingId = borrowingDetailVM.BorrowingId });
        }

        private static int CalculateFine(DateOnly dueTime, DateOnly returnTime)
        {
            var daysLate = returnTime.DayNumber - dueTime.DayNumber;
            Console.WriteLine(daysLate);
            if (daysLate > 0)
            {
                return daysLate <= 10 ? 2 : 4;
            }
            return 0;
        }

        #region API Calls
        private async Task<BorrowingDetailVM> GetBorrowingDetailByIdAsync(int id)
        {
            BorrowingDetailVM borrowingDetailVM = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/Borrowing/get-borrowing-detail-by-id/" + id);
                if (Res.IsSuccessStatusCode)
                {
                    var borrowingDetailResponse = Res.Content.ReadAsStringAsync().Result;
                    borrowingDetailVM = JsonConvert.DeserializeObject<BorrowingDetailVM>(borrowingDetailResponse);
                }
            }
            return borrowingDetailVM;
        }

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

        private async Task<BorrowingDetailVM> UpdateBorrowingDetailById(int id, BorrowingDetailVM borrowingDetailVM)
        {
            BorrowingDetailVM borrowingDetail = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(JsonConvert.SerializeObject(borrowingDetailVM), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await httpClient.PutAsync("api/Borrowing/update-borrowing-detail-by-id/" + id, content);
                if (Res.IsSuccessStatusCode)
                {
                    var borrowingDetailResponse = Res.Content.ReadAsStringAsync().Result;
                    borrowingDetail = JsonConvert.DeserializeObject<BorrowingDetailVM>(borrowingDetailResponse);
                }
            }
            return borrowingDetail;
        }

        private async Task<HttpResponseMessage> UpdateBookQuantity(int id, int quantity)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(quantity), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync($"api/Book/update-book-quantity/{id}", content);

            return response;
        }

        private async Task<HistoryVM> AddHistory(HistoryVM historyVM)
        {
            HistoryVM history = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(JsonConvert.SerializeObject(historyVM), Encoding.UTF8, "application/json");
                HttpResponseMessage Res = await httpClient.PostAsync("api/History/add-history", content);
                if (Res.IsSuccessStatusCode)
                {
                    var historyResponse = Res.Content.ReadAsStringAsync().Result;
                    history = JsonConvert.DeserializeObject<HistoryVM>(historyResponse);
                }
            }
            return history;
        }

        private async Task<HttpResponseMessage> UpdateHistory(HistoryVM historyVM)
        {
            using var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var content = new StringContent(JsonConvert.SerializeObject(historyVM), Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync("api/History/update-history", content);

            return response;
        }

        private async Task<HistoryVM> GetHistoryByUserIdAndBookIdAndBorrowedTime(int userId, int bookId, string borrowedTime)
        {
            HistoryVM historyVM = null;
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient
                    .GetAsync($"api/History/get-history-by-user-id/{userId}/book-id/{bookId}/borrowed-time/{borrowedTime}");
                if (Res.IsSuccessStatusCode)
                {
                    var historyResponse = Res.Content.ReadAsStringAsync().Result;
                    historyVM = JsonConvert.DeserializeObject<HistoryVM>(historyResponse);
                }
            }
            return historyVM;
        }

        #endregion
    }
}
