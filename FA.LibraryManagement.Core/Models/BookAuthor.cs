using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>

/// The book author class

/// </summary>

[Table("BookAuthors")]
public class BookAuthor
{
    /// <summary>
    /// Gets or sets the value of the book id
    /// </summary>
    public int BookId { get; set; }
    /// <summary>
    /// Gets or sets the value of the author id
    /// </summary>
    public int AuthorId { get; set; }

    /// <summary>
    /// Gets or sets the value of the book
    /// </summary>
    public virtual Book Book { get; set; }
    /// <summary>
    /// Gets or sets the value of the author
    /// </summary>
    public virtual Author Author { get; set; }
}