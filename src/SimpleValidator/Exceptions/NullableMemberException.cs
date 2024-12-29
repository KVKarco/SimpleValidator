namespace SimpleValidator.Exceptions;

/// <summary>
/// When predicate contains null members.
/// </summary>
public class NullableMemberException : ValidatorArgumentException
{
    /// <summary>
    /// Creates PredicateWithNullMemberException with message.
    /// </summary>
    public NullableMemberException(string message) : base(message)
    {
    }

    /// <summary>
    /// Creates PredicateWithNullMemberException with message, and inner exception.
    /// </summary>
    public NullableMemberException(string message, Exception innerException) : base(message, innerException)
    {
    }

    /// <summary>
    /// Creates empty PredicateWithNullMemberException.
    /// </summary>
    public NullableMemberException()
    {
    }
}
