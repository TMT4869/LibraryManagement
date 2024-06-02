using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    public UserController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    [HttpPost("get-all-users-by-paging")]
    public IActionResult GetAllUsersByPaging()
    {
        var draw = Request.Form["draw"].FirstOrDefault();
        var start = Request.Form["start"].FirstOrDefault();
        var length = Request.Form["length"].FirstOrDefault();
        var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"]
            .FirstOrDefault();
        var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
        var searchValue = Request.Form["search[value]"].FirstOrDefault();

        var pageSize = length != null ? Convert.ToInt32(length) : 0;
        var skip = start != null ? Convert.ToInt32(start) : 0;
        var totalRecords = 0;

        var users = _unitOfWork.UserRepository.GetPaged(skip, pageSize, searchValue, sortColumn, sortColumnDirection);

        var data = users
            .Results
            .Select((user, index) => new UserVM()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Number = index + 1 + skip,
                FullName = $"{user.FirstName} {user.LastName}",
                PhoneNumber = user.PhoneNumber,
                LockoutEnabled = user.LockoutEnabled,
                ImageUrl = GetImageUrl(user),
                Role = user.UserRoles.FirstOrDefault()?.Role.Name
            });

        return Ok(new
        {
            draw,
            recordsFiltered = users.TotalRecords,
            recordsTotal = users.TotalRecords,
            data
        });
    }

    private string GetImageUrl(User user)
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

    [HttpPost("change-status/{userId}")]
    public async Task<IActionResult> ChangeStatus(int userId)
    {
        var user = _unitOfWork.UserRepository.Find(userId);
        if (user == null)
            return NotFound();

        user.LockoutEnabled = !user.LockoutEnabled;
        user.LockoutEnd = user.LockoutEnabled ? DateTimeOffset.MaxValue : null;
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.SaveChanges();
        if (result <= 0)
            return BadRequest(new
            {
                success = false,
                message = "Failed !"
            });
        return Ok(new
        {
            success = true,
            message = "Success !"
        });
    }

    [HttpPost("delete-user/{userId}")]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        var user = _unitOfWork.UserRepository.Find(userId);
        if (user == null)
            return NotFound();

        _unitOfWork.UserRepository.Delete(user);
        var result = _unitOfWork.SaveChanges();
        if (result <= 0)
            return BadRequest(new
            {
                success = false,
                message = "Failed !"
            });
        return Ok(new
        {
            success = true,
            message = "Success !"
        });
    }

    [HttpGet("get-user-by-id/{userId}")]
    public IActionResult GetUserById(int userId)
    {
        var user = _unitOfWork.UserRepository.Get(u => u.Id == userId, includeProperties: "UserRoles");
        if (user == null)
            return NotFound();
        var userVM = _mapper.Map<UserVM>(user);
        return Ok(userVM);
    }

    [HttpPut("update-user")]
    public async Task<IActionResult> UpdateUser(UserVM userVM)
    {
        var user = _mapper.Map<User>(userVM);
        _unitOfWork.UserRepository.Update(user);
        var result = _unitOfWork.SaveChanges();
        if (result <= 0)
            return BadRequest(new
            {
                success = false,
                message = "Failed !"
            });
        return Ok(new
        {
            success = true,
            message = "Success !"
        });
    }

    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser(UserCreateVM userVM)
    {
        var user = _mapper.Map<User>(userVM);
        switch (userVM.Gender.ToLower())
        {
            case "male":
                user.ImageUrl = "/static/images/faces/1.jpg";
                break;
            case "female":
                user.ImageUrl = "/static/images/faces/5.jpg";
                break;
            default:
                user.ImageUrl = "/static/images/faces/7.jpg";
                break;
        }
        var result = await _userManager.CreateAsync(user, userVM.Password);
        if (!result.Succeeded)
        {
            return BadRequest(new
            {
                success = false,
                errors = result.Errors.Select(e => e.Description).ToList()
            });
        }

        return Ok(new
        {
            success = true,
            message = "Success !"
        });
    }

    #region Remote Validation

    [HttpGet("is-email-in-use/{email}")]
    public IActionResult IsEmailInUse(string email)
    {
        var user = _unitOfWork.UserRepository.Get(u => u.Email == email);
        if (user == null)
            return Ok(true);
        return Ok($"Email {email} is already in use.");
    }

    [HttpGet("is-username-in-use/{userName}")]
    public IActionResult IsUserNameInUse(string userName)
    {
        var user = _unitOfWork.UserRepository.Get(u => u.UserName == userName);
        if (user == null)
            return Ok(true);
        return Ok($"User name {userName} is already in use.");
    }

    #endregion
}