using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost("get-all-authors-by-paging")]
    public IActionResult GetAllAuthorsByPaging()
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

        var categories =
            _unitOfWork.AuthorRepository.GetPaged(skip, pageSize, searchValue, sortColumn, sortColumnDirection);

        var data = categories
            .Results
            .Select((category, index) => new CategoryVM
            {
                Id = category.Id,
                Name = category.Name,
                Number = index + 1 + skip
            });

        return Ok(new
        {
            draw,
            recordsFiltered = categories.TotalRecords,
            recordsTotal = categories.TotalRecords,
            data
        });
    }

    [HttpPost("add-author")]
    public IActionResult CreateAuthor([FromBody] AuthorVM authorVM)
    {
        var author = _mapper.Map<Author>(authorVM);
        _unitOfWork.AuthorRepository.Create(author);
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

    [HttpGet("get-all-authors")]
    public IActionResult GetAllAuthors()
    {
        var authors = _unitOfWork.AuthorRepository.GetAll();
        if (authors == null) return NotFound();

        var authorsVM = _mapper.Map<List<AuthorVM>>(authors);
        return Ok(authorsVM);
    }

    [HttpGet("get-author-by-id/{id}")]
    public IActionResult GetAuthor(int id)
    {
        var author = _unitOfWork.AuthorRepository.GetById(id);
        if (author != null)
        {
            var authorVM = _mapper.Map<AuthorVM>(author);
            return Ok(authorVM);
        }

        return NotFound();
    }

    [HttpPost("update-author-by-id/{id}")]
    public IActionResult UpdateAuthor(int id, [FromBody] AuthorVM authorVM)
    {
        var author = _unitOfWork.AuthorRepository.GetById(id);
        if (author != null)
        {
            author.Name = authorVM.Name;
            _unitOfWork.AuthorRepository.Update(author);
            var result = _unitOfWork.SaveChanges();
            if (result > 0)
                return Ok(new
                {
                    success = true,
                    message = "Success !"
                });
        }

        return BadRequest(new
        {
            success = false,
            message = "Failed !"
        });
    }

    [HttpDelete("delete-author-by-id/{id}")]
    public IActionResult DeleteAuthor(int id)
    {
        var author = _unitOfWork.AuthorRepository.GetById(id);
        if (author != null)
        {
            _unitOfWork.AuthorRepository.Delete(author);
            var result = _unitOfWork.SaveChanges();
            if (result > 0)
                return Ok(new
                {
                    success = true,
                    message = "Success !"
                });
        }

        return BadRequest(new
        {
            success = false,
            message = "Failed !"
        });
    }
}