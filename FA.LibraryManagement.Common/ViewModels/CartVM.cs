using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FA.LibraryManagement.Common.ViewModels
{
    public class CartVM
    {
        /// <summary>
        /// Gets or sets the value of the id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the value of the book id
        /// </summary>
        public int BookId { get; set; }
        /// <summary>
        /// Gets or sets the value of the user id
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// Gets or sets the value of the book
        /// </summary>
        [ValidateNever]
        public virtual BookVM BookVM { get; set; }
        /// <summary>
        /// Gets or sets the value of the user
        /// </summary>
        [ValidateNever]
        public virtual UserVM UserVM { get; set; }
    }
}
