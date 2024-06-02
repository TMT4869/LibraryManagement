using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>
///     The category class
/// </summary>
[Table("Categories")]
public class Category
{
    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the value of the name
    /// </summary>
    [Column(TypeName = "nvarchar(50)")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the value of the books
    /// </summary>
    public virtual IEnumerable<Book>? Books { get; set; }
}