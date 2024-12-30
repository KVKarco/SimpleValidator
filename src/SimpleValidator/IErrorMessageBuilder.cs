namespace SimpleValidator;

/// <summary>
/// Builder for attaching error messages to current rule.
/// </summary>
/// <typeparam name="TEntity">Type that current abstract validator is build for.</typeparam>
/// <typeparam name="TProperty">Type of property that been validated.</typeparam>
public interface IErrorMessageBuilder<TEntity, TProperty>
{
    /// <summary>
    /// Attaches error message to the current rule.
    /// </summary>
    IPropertyRulesBuilder<TEntity, TProperty> WithErrorMessage(string errorMessage);
}

/// <inheritdoc/>
public interface IErrorMessageBuilderForPredicateRule<TEntity, TProperty> : IErrorMessageBuilder<TEntity, TProperty>
{
    /// <summary>
    /// Attaches error message factory to the current rule.
    /// </summary>
    IPropertyRulesBuilder<TEntity, TProperty> WithErrorMessage(Func<IValidationContext<TProperty>, string> errorMessageFactory);
}

/// <inheritdoc/>
public interface IErrorMessageBuilderForComparisonRule<TEntity, TProperty> : IErrorMessageBuilder<TEntity, TProperty>
{
    /// <summary>
    /// Attaches error message factory to the current rule.
    /// </summary>
    IPropertyRulesBuilder<TEntity, TProperty> WithErrorMessage(Func<IValidationContext<TEntity, TProperty>, string> errorMessageFactory);
}
