using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FA.LibraryManagement.Web.Areas.Librarian.Controllers;

[Area("Librarian")]
[Route("Librarian/User")]
public class UserController : Controller
{
    private const string BASE_API_URL = "http://localhost:5055/";
    private readonly IMapper _mapper;

    public UserController(IMapper mapper)
    {
        _mapper = mapper;
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
        return View();
    }

    [HttpPost("Create")]
    public IActionResult Create(UserCreateVM userVm)
    {
        if (ModelState.IsValid)
        {
            var (result, errors) = CreateUser(userVm).Result;
            if (result == 1)
            {
                TempData["Message"] = "User created successfully!";
                return RedirectToAction(nameof(List));
            }
            errors?.ForEach(error => ModelState.AddModelError(string.Empty, error));
        }

        return View(userVm);
    }

    private async Task<(int, List<string>)> CreateUser(UserCreateVM userVm)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = await client.PostAsJsonAsync("api/User/create-user", userVm);
        var apiResponse = Res.Content.ReadAsStringAsync().Result;
        var response = JsonConvert.DeserializeObject<ApiResponse>(apiResponse);
        if (Res.IsSuccessStatusCode)
        {
            return (1, null);
        }

        return (0, response.Errors);
    }

    private async Task<UserVM> GetUserById(int userId)
    {
        UserVM userVM = null;
        using var client = new HttpClient();
        //Passing service base url
        client.BaseAddress = new Uri(BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        //Define request data format
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
        HttpResponseMessage Res = await client.GetAsync("api/User/get-user-by-id/" + userId);
        //Checking the response is successful or not which is sent using HttpClient
        if (Res.IsSuccessStatusCode)
        {
            //Storing the response details recieved from web api
            var userResponse = Res.Content.ReadAsStringAsync().Result;
            //Deserializing the response recieved from web api and storing into the Employee list
            userVM = JsonConvert.DeserializeObject<UserVM>(userResponse);
        }

        return userVM;
    }

    private async Task<List<RoleVM>> GetAllRolesAsync()
    {
        List<RoleVM> rolesVMs = null;
        using var client = new HttpClient();
        //Passing service base url
        client.BaseAddress = new Uri(BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        //Define request data format
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //Sending request to find web api REST service resource GetAllEmployees using HttpClient
        HttpResponseMessage Res = await client.GetAsync("api/Role/get-all-roles");
        //Checking the response is successful or not which is sent using HttpClient
        if (Res.IsSuccessStatusCode)
        {
            //Storing the response details recieved from web api
            var roleResponse = Res.Content.ReadAsStringAsync().Result;
            //Deserializing the response recieved from web api and storing into the Employee list
            rolesVMs = JsonConvert.DeserializeObject<List<RoleVM>?>(roleResponse);
        }

        return rolesVMs;
    }

    [HttpPost]
    [Route("Detail/{id}")]
    public async Task<IActionResult> _RoleModalPartial(int id)
    {
        var user = await GetUserById(id);
        var roles = await GetAllRolesAsync();

        var userVM = _mapper.Map<UserVM>(user);

        var roleList = new List<SelectListItem>();
        roleList.AddRange(roles.Select(role => new SelectListItem(role.Name, role.Id.ToString())));

        userVM.Roles = roleList;
        userVM.RoleId = user.RoleId;
        userVM.ImageUrl = GetImageUrl(userVM);
        return PartialView(userVM);
    }

    private string GetImageUrl(UserVM user)
    {
        switch (user.Gender.ToLower())
        {
            case "male":
                return user.ImageUrl ?? "/static/images/faces/1.jpg";
            case "female":
                return user.ImageUrl ?? "/static/images/faces/5.jpg";
            default:
                return user.ImageUrl ?? "/static/images/faces/7.jpg";
        }
    }

    #region Remote Validation

    [AcceptVerbs("Get", "Post")]
    [Route("IsEmailInUse")]
    public async Task<IActionResult> IsEmailInUse(string email)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = await client.GetAsync("api/User/is-email-in-use/" + email);
        if (Res.IsSuccessStatusCode)
        {
            var result = await Res.Content.ReadAsStringAsync();
            var isEmailInUse = JsonConvert.DeserializeObject<string>(result);
            return Json(isEmailInUse);
        }
        return Json(false);
    }

    [AcceptVerbs("Get", "Post")]
    [Route("IsUserNameInUse")]
    public async Task<IActionResult> IsUserNameInUse(string userName)
    {
        using var client = new HttpClient();
        client.BaseAddress = new Uri(BASE_API_URL);
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        HttpResponseMessage Res = await client.GetAsync("api/User/is-username-in-use/" + userName);
        if (Res.IsSuccessStatusCode)
        {
            var result = await Res.Content.ReadAsStringAsync();
            var isUserNameInUse = JsonConvert.DeserializeObject<string>(result);
            return Json(isUserNameInUse);
        }

        return Json(false);
    }

    #endregion
}