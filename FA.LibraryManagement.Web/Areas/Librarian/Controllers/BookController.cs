using FA.LibraryManagement.Common.Helper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FA.LibraryManagement.Web.Areas.Librarian.Controllers;

[Area("Librarian")]
[Route("Librarian/Book")]
public class BookController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BookController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [Route("List")]
    public IActionResult List()
    {
        if (TempData["Message"] is string message)
        {
            ViewBag.Message = message;
        }

        return View();
    }

    [Route("Create")]
    public IActionResult Create()
    {
        var bookVM = new BookVM();

        var categories = GetAllCategoriesAsync().Result;
        var authors = GetAllAuthorsAsync().Result;

        var categorieList = new List<SelectListItem>(categories.Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = c.Id.ToString()
        }));

        var authorList = new List<SelectListItem>(authors.Select(a => new SelectListItem
        {
            Text = a.Name,
            Value = a.Id.ToString()
        }));

        bookVM.CategorieSelectListItems = categorieList;
        bookVM.AuthorSelectListItems = authorList;
        int lastId = GetLastBookId().Result;
        bookVM.IdDb = lastId + 1;
        return View(bookVM);
    }

    [Route("Create")]
    [HttpPost]
    public IActionResult Create(BookVM bookVM, List<IFormFile> files)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            List<BookImage> bookImages = bookVM.BookImages != null ? bookVM.BookImages.ToList() : new List<BookImage>();
            if (files != null)
            {
                foreach (var file in files)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string bookPath = @"images\books\book-" + bookVM.IdDb;
                    string finalPath = Path.Combine(wwwRootPath, bookPath);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    var bookImage = new BookImage
                    {
                        ImageUrl = @"\" + bookPath + @"\" + fileName,
                    };

                    bookImages.Add(bookImage);
                }

                bookVM.BookAuthors = bookVM.Authors.Select(a => new BookAuthor
                {
                    AuthorId = a
                }).ToList();
                bookVM.BookImages = bookImages;
            }

            var result = CreateBookAsync(bookVM).Result;

            if (result > 0)
            {
                TempData["Message"] = "Book created successfully!";
                return Ok(new
                {
                    success = true
                });
            }
        }

        return View(bookVM);
    }

    [Route("Delete/{bookId}")]
    [HttpDelete]
    public IActionResult Delete(int bookId)
    {
        var result = DeleteBookAsync(bookId).Result;
        if (result <= 0) return RedirectToAction(nameof(List));
        string bookImagePath = @"images\books\book-" + bookId;
        string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, bookImagePath);

        if (Directory.Exists(finalPath))
        {
            string[] filePaths = Directory.GetFiles(finalPath);
            foreach (string filePath in filePaths)
            {
                System.IO.File.Delete(filePath);
            }

            Directory.Delete(finalPath);
        }

        return Ok(new
        {
            success = true,
            message = "Success !"
        });
    }


    [Route("Edit")]
    [HttpGet]
    public IActionResult Edit(int bookId)
    {
        var bookVM = GetBookById(bookId).Result;
        var categories = GetAllCategoriesAsync().Result;
        var authors = GetAllAuthorsAsync().Result;

        var categorieList = new List<SelectListItem>(categories.Select(c => new SelectListItem
        {
            Text = c.Name,
            Value = c.Id.ToString()
        }));
        bookVM.CategorieSelectListItems = categorieList;

        var authorSelected = bookVM.BookAuthors.Select(ba => ba.Author).ToList();

        var authorList = new List<SelectListItem>(authors.Select(a => new SelectListItem
        {
            Text = a.Name,
            Value = a.Id.ToString()
        }));
        authorList.ForEach(a => a.Selected = authorSelected.Any(asel => asel.Id.ToString() == a.Value));

        bookVM.AuthorSelectListItems = authorList;

        return View(bookVM);
    }

    [Route("Edit")]
    [HttpPost]
    public IActionResult Edit(BookVM bookVM, List<IFormFile> files)
    {
        var book = GetBookById(bookVM.Id).Result;

        book.BookAuthors = bookVM.Authors.Select(a => new BookAuthor
        {
            AuthorId = a,
            BookId = book.Id
        }).ToList();
        book.CategoryId = bookVM.CategoryId;
        book.Description = bookVM.Description;
        book.Publisher = bookVM.Publisher;
        book.Quantity = bookVM.Quantity;
        book.Title = bookVM.Title;
        book.PublishedDate = bookVM.PublishedDate;
        book.Authors = bookVM.Authors;
        book.ISBN = bookVM.ISBN;

        string wwwRootPath = _webHostEnvironment.WebRootPath;
        List<BookImage> bookImages = book.BookImages != null ? book.BookImages.ToList() : new List<BookImage>();
        if (ModelState.IsValid)
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string bookPath = @"images\books\book-" + book.Id;
                    string finalPath = Path.Combine(wwwRootPath, bookPath);

                    if (!Directory.Exists(finalPath))
                        Directory.CreateDirectory(finalPath);

                    using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    var bookImage = new BookImage
                    {
                        BookId = book.Id,
                        ImageUrl = @"\" + bookPath + @"\" + fileName,
                    };

                    bookImages.Add(bookImage);
                }

                book.BookImages = bookImages;
            }

            var result = UpdateBookAsync(book).Result;

            if (result > 0)
            {
                TempData["Message"] = "Book updated successfully!";
                return Ok(new
                {
                    success = true
                });
            }
        }

        return View(bookVM);
    }

    [Route("DeleteImage/{imageId}")]
    public IActionResult DeleteImage(int imageId)
    {
        var bookImage = GetBookImageById(imageId).Result;
        if (bookImage == null) return NotFound();

        var bookId = bookImage.BookId;
        if (!string.IsNullOrEmpty(bookImage.ImageUrl))
        {
            var oldImagePath =
                Path.Combine(_webHostEnvironment.WebRootPath,
                    bookImage.ImageUrl.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
        }

        int result = DeleteImageAsync(imageId).Result;

        if (result > 0)
        {
            TempData["Message"] = "Deleted image successfully";
        }

        return RedirectToAction("Edit", "Book", new { area = "Librarian", bookId = bookId });
    }

    #region API Calls

    public async Task<int> GetLastBookId()
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = await client.GetAsync("api/Book/get-last-book-id");
        if (Res.IsSuccessStatusCode)
        {
            var result = Res.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<int>(result);
        }

        return 0;
    }

    private async Task<int> CreateBookAsync(BookVM bookVM)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = await client.PostAsJsonAsync($"api/Book/add-book", bookVM);
        if (Res.IsSuccessStatusCode)
        {
            return 1;
        }

        return 0;
    }

    private async Task<int> DeleteImageAsync(int imageId)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = await client.DeleteAsync($"api/BookImage/delete-book-image-by-id/{imageId}");
        if (Res.IsSuccessStatusCode)
        {
            return 1;
        }

        return 0;
    }

    private async Task<BookImageVM> GetBookImageById(int id)
    {
        BookImageVM bookImageVM = null;
        using var client = new HttpClient();
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        //Define request data format
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.GetAsync($"api/BookImage/get-book-image-by-id/{id}");
        if (response.IsSuccessStatusCode)
        {
            var result = response.Content.ReadAsStringAsync().Result;
            bookImageVM = JsonConvert.DeserializeObject<BookImageVM>(result);
            return bookImageVM;
        }

        return bookImageVM;
    }


    private async Task<int> UpdateBookAsync(BookVM bookVM)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = await client.PutAsJsonAsync($"api/Book/update-book-by-id/{bookVM.Id}", bookVM);
        if (Res.IsSuccessStatusCode)
        {
            return 1;
        }

        return 0;
    }

    private async Task<BookVM> GetBookById(int id)
    {
        BookVM bookVM = null;
        using var client = new HttpClient();
        client.BaseAddress = new Uri("http://localhost:5055/");
        client.DefaultRequestHeaders.Clear();
        //Define request data format
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await client.GetAsync($"api/Book/get-book-by-id/{id}/");
        if (response.IsSuccessStatusCode)
        {
            var result = response.Content.ReadAsStringAsync().Result;
            bookVM = JsonConvert.DeserializeObject<BookVM>(result);
            return bookVM;
        }

        return bookVM;
    }

    private async Task<List<CategoryVM>> GetAllCategoriesAsync()
    {
        List<CategoryVM> categoryVms = null;
        using var client = new HttpClient();
        //Passing service base url
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        //Define request data format
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
        HttpResponseMessage Res = await client.GetAsync("api/Category/get-all-categories");
        //Checking the response is successful or not which is sent using HttpClient
        if (Res.IsSuccessStatusCode)
        {
            //Storing the response details recieved from web api
            var roleResponse = Res.Content.ReadAsStringAsync().Result;
            //Deserializing the response recieved from web api and storing into the Employee list
            categoryVms = JsonConvert.DeserializeObject<List<CategoryVM>?>(roleResponse);
        }

        return categoryVms;
    }

    private async Task<List<AuthorVM>> GetAllAuthorsAsync()
    {
        List<AuthorVM> AuthorVMs = null;
        using var client = new HttpClient();
        //Passing service base url
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        //Define request data format
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
        HttpResponseMessage Res = await client.GetAsync("api/Author/get-all-authors");
        //Checking the response is successful or not which is sent using HttpClient
        if (Res.IsSuccessStatusCode)
        {
            //Storing the response details recieved from web api
            var roleResponse = Res.Content.ReadAsStringAsync().Result;
            //Deserializing the response recieved from web api and storing into the Employee list
            AuthorVMs = JsonConvert.DeserializeObject<List<AuthorVM>?>(roleResponse);
        }

        return AuthorVMs;
    }

    #endregion

    #region Remote Validation

    [HttpGet]
    [AcceptVerbs("Get", "Post")]
    public async Task<IActionResult> IsISBNInUse(string isbn)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(Constant.BASE_API_URL); // Replace with your API base URL
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var response = await client.GetAsync($"api/Book/check-isbn-in-use/{isbn}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            var isISBNInUse = JsonConvert.DeserializeObject<string>(result);
            return Json(isISBNInUse);
        }

        return Json(false);
    }

    private async Task<int> DeleteBookAsync(int bookId)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(Constant.BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = client.DeleteAsync($"api/Book/delete-book-by-id/{bookId}").Result;
        if (Res.IsSuccessStatusCode)
        {
            return 1;
        }

        return 0;
    }

    #endregion
}