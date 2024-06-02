using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FA.LibraryManagement.Common.ViewModels;

/// <summary>
///     The category vm class
/// </summary>
public class CategoryVM
{
    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    [ValidateNever]
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the value of the category name
    /// </summary>
    [Required(ErrorMessage = "Category name is required")]
    [MaxLength(50, ErrorMessage = "Category name can not be more than 50 characters")]
    public string Name { get; set; }

    [ValidateNever]
    public int Number { get; set; }
}