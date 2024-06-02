using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>

/// The user claim class

/// </summary>

/// <seealso cref="IdentityUserClaim{int}"/>

[Table("UserClaims")]
public class UserClaim : IdentityUserClaim<int>
{
    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public int UserId { get; set; }
    /// <summary>
    /// Gets or sets the value of the user
    /// </summary>
    public virtual User User { get; set; } = null!;
}