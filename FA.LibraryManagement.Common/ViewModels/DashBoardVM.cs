namespace FA.LibraryManagement.Common.ViewModels;

public class DashBoardVM
{
    public int TotalBooks { get; set; }
    public int TotalPendingBooks { get; set; }
    public int TotalInCompleteBooks { get; set; }
    public int TotalCompletedBooks { get; set; }
    public int TotalCancelledBooks { get; set; }
    public int TotalBorrowingBooks { get; set; }
    public int TotalReturnedBooks { get; set; }
    public int TotalMembers { get; set; }
    public float TotalFine { get; set; }
    public List<UserVM> UserList { get; set; }
    public List<BookVM> BookList { get; set; }
    public IList<BorrowingTodayListVM> BorrowingTodayList { get; set; }
}