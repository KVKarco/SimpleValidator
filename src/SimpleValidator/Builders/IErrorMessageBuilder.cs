namespace SimpleValidator.Builders;

/// <summary>
/// Builder for attaching error messages to current rule.
/// </summary>
/// <typeparam name="TMainEntity">Type that current abstract validator is build for.</typeparam>
/// <typeparam name="TProperty">Type of property that been validated.</typeparam>
public interface IErrorMessageBuilder<TMainEntity, TProperty>
{
    /// <summary>
    /// Attaches error message factory to the current rule.
    /// </summary>
    IPropertyRulesBuilder<TMainEntity, TProperty> WithErrorMessage(Func<string, TMainEntity, TProperty, string> errorMessageFactory);

    /// <summary>
    /// Attaches error message to the current rule.
    /// </summary>
    IPropertyRulesBuilder<TMainEntity, TProperty> WithErrorMessage(string errorMessage);
}