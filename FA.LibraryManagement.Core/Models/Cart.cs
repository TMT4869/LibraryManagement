using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>

/// The cart class

/// </summary>

[Table("Carts")]
public class Cart
{
    /// <summary>
    /// Gets or sets the value of the id
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Gets or sets the value of the book id
    /// </summary>
    public int BookId { get; set; }
    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// Gets or sets the value of the book
    /// </summary>
    public virtual Book Book { get; set; }
    /// <summary>
    /// Gets or sets the value of the user
    /// </summary>
    public virtual User User { get; set; }
}