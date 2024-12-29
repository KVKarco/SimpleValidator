namespace SimpleValidator.Exceptions;

/// <summary>
/// Custom exception when selector expression is null wrong or for nested properties. 
/// </summary>
public class InvalidSelectorException : ValidatorArgumentException
{
    /// <summary>
    /// Creates InvalidSelectorException with message.
    /// </summary>
    public InvalidSelectorException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates InvalidSelectorException with message, and inner exception.
    /// </summary>
    public InvalidSelectorException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates empty InvalidSelectorException.
    /// </summary>
    public InvalidSelectorException()
    {
    }
}
