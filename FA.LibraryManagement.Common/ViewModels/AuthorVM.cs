using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace FA.LibraryManagement.Common.ViewModels;

public class AuthorVM
{
    /// <summary>
    ///     Gets or sets the value of the id
    /// </summary>
    [ValidateNever]
    public int Id { get; set; }

    /// <summary>
    ///     Gets or sets the value of the category name
    /// </summary>
    [Required(ErrorMessage = "Author name is required")]
    [MaxLength(50, ErrorMessage = "Author name can not be more than 50 characters")]
    public string Name { get; set; }
}