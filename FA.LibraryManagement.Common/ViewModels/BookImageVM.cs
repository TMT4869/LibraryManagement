using FA.LibraryManagement.Core.Models;

namespace FA.LibraryManagement.Common.ViewModels;

public class BookImageVM
{
    public int Id { get; set; }
    public string ImageUrl { get; set; }
    public int BookId { get; set; }
    public virtual Book Book { get; set; }
}