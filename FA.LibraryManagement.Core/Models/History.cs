using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>
///     The history class
/// </summary>
[Table("Histories")]
public class History
{
    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book id
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the user id
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the borrowed time
    /// </summary>
    public DateOnly BorrowedTime { get; set; }

    /// <summary>
    ///     Gets or sets the value of the returned time
    /// </summary>
    public DateOnly? ReturnedTime { get; set; }

    /// <summary>
    ///     Gets or sets the value of the due time
    /// </summary>
    public DateOnly? DueTime { get; set; }

    /// <summary>
    ///     Gets or sets the value of the fine
    /// </summary>
    public float Fine { get; set; }

    /// <summary>
    ///     Gets or sets the value of the status
    /// </summary>
    [Column(TypeName = "nvarchar(10)")]
    public string Status { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book
    /// </summary>
    public virtual Book Book { get; set; }

    /// <summary>
    ///     Gets or sets the value of the user
    /// </summary>
    public virtual User User { get; set; }
}