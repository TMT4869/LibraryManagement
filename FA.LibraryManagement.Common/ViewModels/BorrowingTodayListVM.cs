namespace FA.LibraryManagement.Common.ViewModels;

public class BorrowingTodayListVM
{
    public int BorrowingId { get; set; }
    public string ImageUrl { get; set; }
    public string UserName { get; set; }
    public string Gender { get; set; }
    public int NumberOfBooks { get; set; }
    public DateOnly BorrowedTime { get; set; }
    public DateOnly ReturnTime { get; set; }
    public DateOnly DueTime { get; set; }
    public float Fine { get; set; }
}