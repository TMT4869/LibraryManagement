using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using X.PagedList;

namespace FA.LibraryManagement.Web.Areas.Member.Controllers
{
    [Area("Member")]
    [Route("Category")]
    public class CategoryController : Controller
    {
        public IActionResult Index([FromQuery(Name = "categoryId")] int categoryId, [FromQuery(Name = "page")] int page = 1, [FromQuery(Name = "keyword")] string keyword = "")
        {
            var result = GetBooksAsync(categoryId, page, keyword).Result;

            if (result == null)
            {
                return NotFound();
            }

            var pagedList = new StaticPagedList<BookVM>(result.Items, page, result.PageSize, result.TotalCount);

            ViewData["categoryId"] = categoryId;
            ViewData["keyword"] = keyword;

            return View(pagedList);
        }

        private async Task<ApiPagedResult<BookVM>> GetBooksAsync(int categoryId, int page = 1, string keyword = "")
        {
            string baseUrl = "http://localhost:5055/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res =
                    await httpClient.GetAsync(
                        $"api/Category/get-books-by-category?categoryId={categoryId}&page={page}&keyword={keyword}");
                if (Res.IsSuccessStatusCode)
                {
                    var bookResponse = Res.Content.ReadAsStringAsync().Result;
                    ApiPagedResult<BookVM> result = JsonConvert.DeserializeObject<ApiPagedResult<BookVM>>(bookResponse);
                    return result;
                }

                return null;
            }
        }
    }
}