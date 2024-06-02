using AutoMapper;
using FA.LibraryManagement.API.ViewModels;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoleController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("get-all-roles")]
    public async Task<IActionResult> GetAll()
    {
        var roles = _unitOfWork.RoleRepository.GetAll();
        var rolesVM = _mapper.Map<List<RoleVM>>(roles);
        return Ok(rolesVM);
    }

    [HttpPut("update-role-for-user")]
    public async Task<IActionResult> UpdateRoleForUser(UserRoleVM userRoleVM)
    {
        var user = _unitOfWork.UserRepository.GetById(userRoleVM.UserId);
        if (user == null)
        {
            return NotFound();
        }
        var role = _unitOfWork.RoleRepository.GetById(userRoleVM.RoleId);
        if (role == null)
        {
            return NotFound();
        }

        _unitOfWork.RoleUserRepository.Update(user, role);
        var result = _unitOfWork.SaveChanges();
        if (result > 0)
            return Ok(new
            {
                success = true,
                message = "Success !"
            });

        return BadRequest(new
        {
            success = false,
            message = "Failed !"
        });
    }
}