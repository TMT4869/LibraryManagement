using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FA.LibraryManagement.Common.ViewModels;
public class RoleVM
{
    /// <summary>
    /// Gets or sets the value of the role id
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the value of the description
    /// </summary>
    [Required(ErrorMessage = "Author name is required")]
    [MaxLength(50, ErrorMessage = "Author name can not be more than 50 characters")]
    public string Name { get; set; }

    [ValidateNever]
    public List<SelectList> Roles { get; set; }
}