using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FA.LibraryManagement.Common.ViewModels
{
    public class BorrowingDetailVM
    {
        public int Id { get; set; }
        public int BorrowingId { get; set; }
        public int BookId { get; set; }
        public DateOnly DueTime { get; set; }
        public DateOnly ReturnTime { get; set; }
        public string Status { get; set; }
        public float Fine { get; set; }
        [ValidateNever]
        public BookVM BookVM { get; set; }
    }
}
