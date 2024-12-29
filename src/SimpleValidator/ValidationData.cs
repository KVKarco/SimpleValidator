namespace SimpleValidator;

/// <summary>
/// Data needed for creating error message.
/// </summary>
/// <typeparam name="TEntity">Main validation type value, 
/// if necessary to display a comparison value in the error message.</typeparam>
/// <typeparam name="TProperty">The property value that's been validated, 
/// if necessary to display a comparison value in the error message. </typeparam>
public sealed class ValidationData<TEntity, TProperty>
{
    /// <summary>
    /// Create new ErrorMsgData
    /// </summary>
    /// <param name="propertyName">TProperty name.</param>
    /// <param name="entityValue">TEntity value.</param>
    /// <param name="propertyValue">TProperty value.</param>
    public ValidationData(string propertyName, TEntity entityValue, TProperty propertyValue)
    {
        PropertyName = propertyName;
        EntityValue = entityValue;
        PropertyValue = propertyValue;
    }

    /// <summary>
    /// The property name that's been validated
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Main validation type value.
    /// </summary>
    public TEntity EntityValue { get; }

    /// <summary>
    /// The property value that's been validated
    /// </summary>
    public TProperty PropertyValue { get; }
}
