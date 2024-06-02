using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>
///     The user class
/// </summary>
/// <seealso cref="IdentityUser{}" />
[Table("Users")]
public class User : IdentityUser<int>
{
    /// <summary>
    ///     Gets or sets the value of the first name
    /// </summary>
    [Column(TypeName = "nvarchar(50)")]
    public string? FirstName { get; set; }

    /// <summary>
    ///     Gets or sets the value of the last name
    /// </summary>
    [Column(TypeName = "nvarchar(50)")]
    public string? LastName { get; set; }

    /// <summary>
    ///     Gets or sets the value of the birth date
    /// </summary>
    public DateTime? BirthDate { get; set; }

    /// <summary>
    ///     Gets or sets the value of the gender
    /// </summary>
    [Column(TypeName = "nvarchar(30)")]
    public string? Gender { get; set; }

    /// <summary>
    ///     Gets or sets the value of the image url
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    ///     Gets or sets the value of the claims
    /// </summary>
    public virtual IEnumerable<UserClaim> Claims { get; set; }

    /// <summary>
    ///     Gets or sets the value of the logins
    /// </summary>
    public virtual IEnumerable<UserLogin> Logins { get; set; }

    /// <summary>
    ///     Gets or sets the value of the tokens
    /// </summary>
    public virtual IEnumerable<UserToken> Tokens { get; set; }

    /// <summary>
    ///     Gets or sets the value of the user roles
    /// </summary>
    public virtual IEnumerable<UserRole> UserRoles { get; set; }

    /// <summary>
    ///     Gets or sets the value of the orders
    /// </summary>
    public virtual IEnumerable<Borrowing>? Borrowings { get; set; }

    /// <summary>
    ///     Gets or sets the value of the histories
    /// </summary>
    public virtual IEnumerable<History>? Histories { get; set; }

    /// <summary>
    ///     Gets or sets the value of the carts
    /// </summary>
    public virtual IEnumerable<Cart>? Carts { get; set; }
}