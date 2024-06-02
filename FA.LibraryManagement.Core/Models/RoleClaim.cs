using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>

/// The role claim class

/// </summary>

/// <seealso cref="IdentityRoleClaim{int}"/>

[Table("RoleClaims")]
public class RoleClaim : IdentityRoleClaim<int>
{
    /// <summary>
    /// Gets or sets the value of the role id
    /// </summary>
    public int RoleId { get; set; }
    /// <summary>
    /// Gets or sets the value of the role
    /// </summary>
    public virtual Role Role { get; set; } = null!;
    /// <summary>
    /// Gets or sets the value of the user id
    /// </summary>
    public int UserId { get; set; }
}