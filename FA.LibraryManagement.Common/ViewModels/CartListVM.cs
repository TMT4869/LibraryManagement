namespace FA.LibraryManagement.Common.ViewModels
{
    public class CartListVM
    {
        public IEnumerable<CartVM> Carts { get; set; }
        public BorrowingVM BorrowingVM { get; set; }
    }
}
