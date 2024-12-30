namespace SimpleValidator;

/// <summary>
/// Represent collection of data needed for the current validation run.
/// </summary>
public interface IValidationContext
{
    /// <summary>
    /// Name of the current property that been validated.
    /// </summary>
    string PropertyName { get; }
}

/// <inheritdoc/>
/// <typeparam name="TProperty">Type of property that been validated.</typeparam>
public interface IValidationContext<TProperty> : IValidationContext
{
    /// <summary>
    /// Value of the current property that been validated.
    /// </summary>
    TProperty PropertyValue { get; }
}

/// <inheritdoc/>
/// <typeparam name="TEntity">The main type that AbstractValidator is build for.</typeparam>
/// <typeparam name="TProperty">Type of property that been validated.</typeparam>
public interface IValidationContext<TEntity, TProperty> : IValidationContext<TProperty>
{
    /// <summary>
    /// Value of the main type that AbstractValidator is build for.
    /// </summary>
    TEntity EntityValue { get; }
}
