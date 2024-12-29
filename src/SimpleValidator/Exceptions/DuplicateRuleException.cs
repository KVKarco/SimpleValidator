namespace SimpleValidator.Exceptions;

/// <summary>
/// Custom exception when duplicate rule is added to IPropertyValidator
/// </summary>
public sealed class DuplicateRuleException : ValidatorArgumentException
{
    /// <summary>
    /// Creates RuleDuplicateException with message.
    /// </summary>
    public DuplicateRuleException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates RuleDuplicateException with message, and inner exception.
    /// </summary>
    public DuplicateRuleException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates empty RuleDuplicateException.
    /// </summary>
    public DuplicateRuleException()
    {
    }
}