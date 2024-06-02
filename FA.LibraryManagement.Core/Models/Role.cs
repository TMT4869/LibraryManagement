using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Core.Models;

/// <summary>

/// The role class

/// </summary>

/// <seealso cref="IdentityRole{int}"/>

[Table("Roles")]
public class Role : IdentityRole<int>
{
    /// <summary>
    /// Gets or sets the value of the role id
    /// </summary>
    [Key] public int RoleId { get; set; }

    /// <summary>
    /// Gets or sets the value of the description
    /// </summary>
    public string Description { get; set; }
    /// <summary>
    /// Gets or sets the value of the users
    /// </summary>
    public virtual IEnumerable<User>? Users { get; set; }
    /// <summary>
    /// Gets or sets the value of the user roles
    /// </summary>
    public virtual IEnumerable<UserRole> UserRoles { get; set; }
    /// <summary>
    /// Gets or sets the value of the role claims
    /// </summary>
    public virtual IEnumerable<RoleClaim> RoleClaims { get; set; }
}