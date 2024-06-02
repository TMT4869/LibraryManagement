using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace FA.LibraryManagement.Common.ViewModels
{
    public class BorrowingVM
    {
        /// <summary>
        ///     Gets or sets the value of the id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Gets or sets the value of the user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        ///     Gets or sets the value of the borrowed time
        /// </summary>
        public DateOnly BorrowedTime { get; set; }

        /// <summary>
        ///     Gets or sets the value of the status
        /// </summary>
        [Column(TypeName = "nvarchar(10)")]
        public string Status { get; set; }

        /// <summary>
        ///     Gets or sets the value of the user
        /// </summary>
        [ValidateNever]
        public virtual UserVM UserVM { get; set; }

        /// <summary>
        ///     Gets or sets the value of the order details
        /// </summary>
        /// 
        public virtual IEnumerable<BorrowingDetailVM>? BorrowingDetailsVM { get; set; }
    }
}
