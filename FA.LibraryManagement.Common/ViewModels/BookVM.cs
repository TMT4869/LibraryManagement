using FA.LibraryManagement.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Common.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the value of the category id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        ///     Gets or sets the value of the isbn
        /// </summary>
        [Column(TypeName = "nvarchar(20)")]
        [Required]
        [RegularExpression("^(?=(?:[^0-9]*[0-9]){10}(?:(?:[^0-9]*[0-9]){3})?$)[\\d-]+$", ErrorMessage = "Invalid ISBN format")]
        [Remote("IsISBNInUse", "Book", "Librarian")]
        public string ISBN { get; set; }

        /// <summary>
        ///     Gets or sets the value of the title
        /// </summary>
        [Column(TypeName = "nvarchar(250)")]
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        /// <summary>
        ///     Gets or sets the value of the description
        /// </summary>

        [Display(Name = "Description")]
        [Required]
        public string Description { get; set; }

        /// <summary>
        ///     Gets or sets the value of the publisher
        /// </summary>
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string Publisher { get; set; }

        /// <summary>
        ///     Gets or sets the value of the quantity
        /// </summary>
        [Required]
        public int Quantity { get; set; }

        /// <summary>
        ///     Gets or sets the value of the category
        /// </summary>
        [ValidateNever]
        [Display(Name = "Category")]
        public string? CategoryName { get; set; }

        /// <summary>
        ///     Gets or sets the value of the author
        /// </summary>
        [ValidateNever]
        public virtual IEnumerable<int>? Authors { get; set; }

        [ValidateNever]
        public virtual IEnumerable<string>? AuthorNames { get; set; }

        [ValidateNever]
        public virtual IEnumerable<BookImage>? BookImages { get; set; }

        [ValidateNever]
        public virtual IEnumerable<BookAuthor>? BookAuthors { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [ValidateNever]
        public string AuthorName { get; set; }

        [Required]
        public DateOnly PublishedDate { get; set; }

        [ValidateNever]
        public string PublishedDateString { get; set; }

        [ValidateNever]
        public int Number { get; set; }

        [ValidateNever]
        public List<SelectListItem> CategorieSelectListItems { get; set; }

        [ValidateNever]
        public List<SelectListItem> AuthorSelectListItems { get; set; }

        public bool IsExistCart = false;

        [ValidateNever]
        public int IdDb { get; set; }

    }
}
