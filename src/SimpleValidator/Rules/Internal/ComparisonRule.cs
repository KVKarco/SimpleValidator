namespace SimpleValidator.Rules.Internal;

using SimpleValidator.Internal;

/// <summary>
/// Validation rule from func with comparison values.
/// </summary>
internal sealed class ComparisonRule<TEntity, TProperty> : IValidationRule<TEntity, TProperty>
{
    private readonly Func<TEntity, TProperty, bool> predicate;

    public ComparisonRule(Func<TEntity, TProperty, bool> predicate, string ruleName)
    {
        this.predicate = predicate;
        this.RuleName = ruleName;
    }

    public string RuleName { get; }

    /// <inheritdoc />
    public bool FailsWhen(TEntity entityValue, TProperty propertyValue) => this.predicate(entityValue, propertyValue);

    /// <inheritdoc />
    public string GetDefaultMsgTemplate(string propName, TEntity entityValue, TProperty propertyValue)
        => $"{DefaultErrorMessages.GenericErrorMsg} {propName}.";
}
