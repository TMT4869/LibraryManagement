using AutoMapper;
using FA.LibraryManagement.Common.ViewModels;
using FA.LibraryManagement.Core.Infrastructers;
using Microsoft.AspNetCore.Mvc;

namespace FA.LibraryManagement.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookImageController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public BookImageController(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [HttpGet("get-book-image-by-id/{id}")]
    public IActionResult GetBookImage(int id)
    {
        var bookImage = _unitOfWork.BookImageRepository.Get(bi => bi.Id == id);
        if (bookImage != null)
        {
            var bookImageVM = _mapper.Map<BookImageVM>(bookImage);
            return Ok(bookImageVM);
        }

        return NotFound();
    }

    [HttpDelete("delete-book-image-by-id/{id}")]
    public IActionResult DeleteBookImage(int id)
    {
        var bookImage = _unitOfWork.BookImageRepository.Get(bi => bi.Id == id);
        if (bookImage == null) return NotFound();

        _unitOfWork.BookImageRepository.Delete(bookImage);
        var result = _unitOfWork.SaveChanges();
        if (result > 0)
            return Ok();

        return BadRequest();
    }
}