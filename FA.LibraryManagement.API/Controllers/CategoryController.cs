using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace FA.LibraryManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CategoryController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpPost("get-all-categories-by-paging")]
    public IActionResult GetAllCategoriesByPaging()
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
            _unitOfWork.CategoryRepository.GetPaged(skip, pageSize, searchValue, sortColumn, sortColumnDirection);

        var data = categories
            .Results
            .Select((category, index) => new CategoryVM()
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


    [HttpPost("add-category")]
    public IActionResult CreateCategory([FromBody] CategoryVM categoryVM)
    {
        var category = _mapper.Map<Category>(categoryVM);
        _unitOfWork.CategoryRepository.Create(category);
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

    [HttpGet("get-all-categories")]
    public IActionResult GetAllCategories()
    {
        var categories = _unitOfWork.CategoryRepository.GetAll();
        if (categories == null) return NotFound();

        var categoriesVM = _mapper.Map<List<CategoryVM>>(categories);
        return Ok(categoriesVM);
    }

    [HttpGet("get-category-by-id/{id}")]
    public IActionResult GetCategory(int id)
    {
        var category = _unitOfWork.CategoryRepository.GetById(id);
        if (category != null)
        {
            var categoryVM = _mapper.Map<CategoryVM>(category);
            return Ok(categoryVM);
        }

        return NotFound();
    }

    [HttpPost("update-category-by-id/{id}")]
    public IActionResult UpdateCategory(int id, [FromBody] CategoryVM categoryVM)
    {
        var category = _unitOfWork.CategoryRepository.GetById(id);
        if (category != null)
        {
            category.Name = categoryVM.Name;
            _unitOfWork.CategoryRepository.Update(category);
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

    [HttpDelete("delete-category-by-id/{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var category = _unitOfWork.CategoryRepository.GetById(id);
        if (category != null)
        {
            _unitOfWork.CategoryRepository.Delete(category);
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

    [HttpGet("get-books-by-category")]
    public IActionResult GetBooksByCategory(int categoryId, int page, string keyword = "")
    {
        var books = _unitOfWork.BookRepository.GetAllBooksByCategory(categoryId, keyword);

        int pageSize = 1;

        if (books != null)
        {
            var bookVM = _mapper.Map<List<BookVM>>(books);

            var pagedList = bookVM.ToPagedList(page, pageSize);

            return Ok(new
            {
                TotalCount = pagedList.TotalItemCount,
                PageNumber = pagedList.PageNumber,
                PageSize = pagedList.PageSize,
                Items = pagedList.ToList()
            });
        }
        return NotFound();
    }
}