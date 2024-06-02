using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>
///     The book image class
/// </summary>
[Table("BookImages")]
public class BookImage
{
    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the value of the image url
    /// </summary>
    public string ImageUrl { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book id
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book
    /// </summary>
    public virtual Book Book { get; set; }
}