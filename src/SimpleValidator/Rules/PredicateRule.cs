using SimpleValidator.Internal;

namespace SimpleValidator.Rules;

/// <summary>
/// Validation rule from predicate.
/// </summary>
internal sealed class PredicateRule<TEntity, TProperty> : IValidationRule<TEntity, TProperty>
{
    private readonly Predicate<TProperty> _predicate;

    public PredicateRule(Predicate<TProperty> predicate, string ruleName)
    {
        _predicate = predicate;
        RuleName = ruleName;
    }

    public string RuleName { get; }

    /// <inheritdoc />
    public bool FailsWhen(TEntity entityValue, TProperty propertyValue) => _predicate(propertyValue);

    /// <inheritdoc />
    public string GetDefaultMsgTemplate(string propName, TEntity entityValue, TProperty propertyValue)
        => $"{DefaultErrorMessages._genericErrorMsg} {propName}.";
}
