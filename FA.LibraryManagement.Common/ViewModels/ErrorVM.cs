using Newtonsoft.Json;

namespace FA.LibraryManagement.Common.ViewModels;

/// <summary>
///     The error vm class
/// </summary>
public class ErrorVM
{
    /// <summary>
    ///     Gets or sets the value of the status code
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    ///     Gets or sets the value of the message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    ///     Gets or sets the value of the path
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    ///     Returns the string
    /// </summary>
    /// <returns>The string</returns>
    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }
}