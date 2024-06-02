using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.Web.Areas.Librarian.Controllers;

[Area("Librarian")]
[Route("Librarian/Category")]
public class CategoryController : Controller
{
    [Route("List")]
    public IActionResult List()
    {
        return View();
    }
}