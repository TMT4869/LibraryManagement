using FA.LibraryManagement.Common.Helper;
using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FA.LibraryManagement.Web.Areas.Librarian.Controllers;

[Area("Librarian")]
[Route("Librarian/Home")]
public class HomeController : Controller
{
    [Route("Index")]
    public IActionResult Index()
    {
        var dashBoarVM = GetSummary().Result;
        return View(dashBoarVM);
    }


    #region API Calls

    public async Task<DashBoardVM> GetSummary()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = await client.GetAsync("api/DashBoard/summary");
        if (Res.IsSuccessStatusCode)
        {
            var content = await Res.Content.ReadAsStringAsync();
            var dashBoardVM = JsonConvert.DeserializeObject<DashBoardVM>(content);
            return dashBoardVM;
        }
        return null;
    }

    #endregion
}