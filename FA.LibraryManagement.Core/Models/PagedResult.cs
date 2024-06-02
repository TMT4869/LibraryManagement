namespace FA.LibraryManagement.Core.Models;

public class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the value of the total records
    /// </summary>
    public int TotalRecords { get; set; }
    /// <summary>
    /// Gets or sets the value of the results
    /// </summary>
    public List<T> Results { get; set; }
}