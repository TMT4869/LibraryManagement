using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>

/// The author class

/// </summary>

[Table("Authors")]
public class Author
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
}