using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FA.LibraryManagement.Web.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(string viewName = "Default")
        {
            var categories = await GetCategoriesAsync();
            return View(viewName, categories);
        }

        private async Task<List<CategoryVM>> GetCategoriesAsync()
        {
            var categories = new List<CategoryVM>();
            string baseUrl = "http://localhost:5055/";
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = await httpClient.GetAsync("api/Category/get-all-categories");
                if (Res.IsSuccessStatusCode)
                {
                    var categoryResponse = await Res.Content.ReadAsStringAsync();
                    categories = JsonConvert.DeserializeObject<List<CategoryVM>>(categoryResponse);
                    return categories;
                }
                return null;
            }
        }

    }
}
