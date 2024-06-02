using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.Web.Areas.Librarian.Controllers;

[Area("Librarian")]
[Route("Librarian/Author")]
public class AuthorController : Controller
{
    [Route("List")]
    public IActionResult List()
    {
        return View();
    }
}