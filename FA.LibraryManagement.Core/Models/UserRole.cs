using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>

/// The user role class

/// </summary>

/// <seealso cref="IdentityUserRole{int}"/>

[Table("UserRoles")]
public class UserRole : IdentityUserRole<int>
{
    /// <summary>
    /// Gets or sets the value of the user
    /// </summary>
    public virtual User User { get; set; } = null!;
    /// <summary>
    /// Gets or sets the value of the role
    /// </summary>
    public virtual Role Role { get; set; } = null!;
}