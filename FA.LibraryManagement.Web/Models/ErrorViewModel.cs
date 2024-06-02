namespace FA.LibraryManagement.Web.Models;

/// <summary>

/// The error view model class

/// </summary>

public class ErrorViewModel
{
    /// <summary>
    /// Gets or sets the value of the request id
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// Gets the value of the show request id
    /// </summary>
    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}