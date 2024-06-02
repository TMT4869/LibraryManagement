using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using X.PagedList;

namespace FA.LibraryManagement.Web.Areas.Member.Controllers
{
    [Area("Member")]
    public class HomeController : Controller
    {
        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1, string keyword = "")
        {

            var result = await GetBooksAsync(page, keyword);

            if (result == null)
            {
                return NotFound();
            }

            var pagedList = new StaticPagedList<BookVM>(result.Items, page, result.PageSize, result.TotalCount);

            ViewData["keyword"] = keyword;

            if (TempData["Message"] is string message)
            {
                ViewBag.Message = message;
            }

            return View(pagedList);
        }

        private async Task<ApiPagedResult<BookVM>> GetBooksAsync(int page = 1, string keyword = "")
        {
            ApiPagedResult<BookVM> result;
            string baseUrl = "http://localhost:5055/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync($"api/Book/get-all-books?page={page}&keyword={keyword}");
                if (Res.IsSuccessStatusCode)
                {
                    var bookResponse = await Res.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<ApiPagedResult<BookVM>>(bookResponse);

                    foreach (var book in result.Items)
                    {
                        if (book.PublishedDate != DateOnly.MinValue)
                        {
                            book.PublishedDateString = book.PublishedDate.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            book.PublishedDateString = string.Empty;
                        }
                    }
                    return result;
                }
                return null;
            }
        }

        private async Task<AuthorVM> GetAuthorNameAsync(int authorId)
        {
            AuthorVM authorVM = null;
            string baseUrl = "http://localhost:5055/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/Author/get-author-by-id/" + authorId);
                if (Res.IsSuccessStatusCode)
                {
                    var bookResponse = Res.Content.ReadAsStringAsync().Result;
                    authorVM = JsonConvert.DeserializeObject<AuthorVM>(bookResponse);

                    return authorVM;
                }
                return null;
            }
        }

        private async Task<List<BookVM>> SearchBooksAsync(string keyword)
        {
            var books = new List<BookVM>();
            string baseUrl = "http://localhost:5055/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/Book/search-books?keyword=" + keyword);
                if (Res.IsSuccessStatusCode)
                {
                    var bookResponse = Res.Content.ReadAsStringAsync().Result;
                    books = JsonConvert.DeserializeObject<List<BookVM>>(bookResponse);
                    foreach (var item in books)
                    {
                        item.AuthorName = GetAuthorNameAsync(item.Authors.FirstOrDefault()).Result.Name;
                    }
                    return books;
                }
                return null;
            }
        }
    }
}
