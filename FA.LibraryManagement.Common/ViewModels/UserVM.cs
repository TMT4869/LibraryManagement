using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FA.LibraryManagement.Common.ViewModels;

public class UserVM
{
    [ValidateNever]
    public int Id { get; set; }

    [ValidateNever]
    public List<SelectListItem> Roles { get; set; }

    [ValidateNever]
    public int RoleId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string Gender { get; set; }
    public string PhoneNumber { get; set; }

    public DateTime BirthDate { get; set; }
    public string? ImageUrl { get; set; }
    public string Role { get; set; }

    [ValidateNever] public string FullName { get; set; }

    [ValidateNever] public int Number { get; set; }

    [ValidateNever] public bool LockoutEnabled { get; set; }
}

public class UserCreateVM
{
    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    [Remote("IsEmailInUse", "User", "Librarian")]
    public string Email { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [Required]
    [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
        MinimumLength = 6)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required]
    [Display(Name = "Username")]
    [Remote("IsUserNameInUse", "User", "Librarian")]
    public string UserName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [Display(Name = "Gender")]
    public string Gender { get; set; }

    [Required]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Required]
    [Display(Name = "Date of Birth")]
    public DateTime BirthDate { get; set; }

    public string? ImageUrl { get; set; }
}