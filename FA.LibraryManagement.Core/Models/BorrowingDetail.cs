using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>
///     The order detail class
/// </summary>
[Table("BorrowingDetails")]
public class BorrowingDetail
{
    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the value of the order id
    /// </summary>
    public int BorrowingId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book id
    /// </summary>
    public int BookId { get; set; }

    /// <summary>
    ///     Gets or sets the value of the due time
    /// </summary>
    public DateOnly DueTime { get; set; }

    /// <summary>
    ///     Gets or sets the value of the return time
    /// </summary>
    public DateOnly ReturnTime { get; set; }

    /// <summary>
    ///     Gets or sets the value of the status
    /// </summary>
    [Column(TypeName = "nvarchar(10)")]
    public string Status { get; set; }

    /// <summary>
    ///     Gets or sets the value of the fine
    /// </summary>
    public float Fine { get; set; }

    /// <summary>
    ///     Gets or sets the value of the order
    /// </summary>
    public virtual Borrowing Borrowing { get; set; }

    /// <summary>
    ///     Gets or sets the value of the book
    /// </summary>
    public virtual Book Book { get; set; }

    #region NotMap in database

    [NotMapped]
    public int NumberOfBooks { get; set; }

    [NotMapped]
    public string UserName { get; set; }

    [NotMapped]
    public DateOnly BorrowedTime { get; set; }

    [NotMapped]
    public string ImageUrl { get; set; }

    [NotMapped]
    public string Gender { get; set; }

    #endregion
}
