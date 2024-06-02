using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>
///     The book class
/// </summary>
[Table("Books")]
public class Book
{
    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the value of the category id
    /// </summary>
    public int CategoryId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the isbn
    /// </summary>
    [Column(TypeName = "nvarchar(20)")]
    public string ISBN { get; set; }

    /// <summary>
    ///     Gets or sets the value of the title
    /// </summary>
    [Column(TypeName = "nvarchar(250)")]
    public string Title { get; set; }

    /// <summary>
    ///     Gets or sets the value of the description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///     Gets or sets the value of the publisher
    /// </summary>
    [Column(TypeName = "nvarchar(100)")]
    public string Publisher { get; set; }

    /// <summary>
    ///     Gets or sets the value of published date
    /// </summary>
    public DateOnly PublishedDate { get; set; }

    /// <summary>
    ///     Gets or sets the value of the quantity
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    ///     Gets or sets the value of the category
    /// </summary>
    public virtual Category Category { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book image
    /// </summary>
    /// <summary>
    ///     Gets or sets the value of the histories
    /// </summary>
    public virtual IEnumerable<History>? Histories { get; set; }

    /// <summary>
    ///     Gets or sets the value of the order details
    /// </summary>
    public virtual IEnumerable<BorrowingDetail>? BorrowingDetails { get; set; }

    /// <summary>
    ///     Gets or sets the value of the carts
    /// </summary>
    public virtual IEnumerable<Cart>? Carts { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book image
    /// </summary>
    public virtual IEnumerable<BookImage>? BookImages { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book author
    /// </summary>
    public virtual IEnumerable<BookAuthor>? BookAuthors { get; set; }
}