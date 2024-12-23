namespace SimpleValidator.Rules.Internal;

using SimpleValidator.Internal;

/// <summary>
/// Validation rule from predicate.
/// </summary>
internal sealed class PredicateRule<TEntity, TProperty> : IValidationRule<TEntity, TProperty>
{
    private readonly Predicate<TProperty> predicate;

    public PredicateRule(Predicate<TProperty> predicate, string ruleName)
    {
        this.predicate = predicate;
        this.RuleName = ruleName;
    }

    public string RuleName { get; }

    /// <inheritdoc />
    public bool FailsWhen(TEntity entityValue, TProperty propertyValue) => this.predicate(propertyValue);

    /// <inheritdoc />
    public string GetDefaultMsgTemplate(string propName, TEntity entityValue, TProperty propertyValue)
        => $"{DefaultErrorMessages.GenericErrorMsg} {propName}.";
}
