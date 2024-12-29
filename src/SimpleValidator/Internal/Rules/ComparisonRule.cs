namespace SimpleValidator.Internal.Rules;

/// <summary>
/// Validation rule from func with comparison values.
/// </summary>
internal sealed class ComparisonRule<TEntity, TProperty> : IValidationRule<TEntity, TProperty>
{
    private readonly Func<TEntity, TProperty, bool> _predicate;

    public ComparisonRule(Func<TEntity, TProperty, bool> predicate, string ruleName)
    {
        _predicate = predicate;
        RuleName = ruleName;
    }

    public string RuleName { get; }

    /// <inheritdoc />
    public bool FailsWhen(TEntity entityValue, TProperty propertyValue) => _predicate(entityValue, propertyValue);

    /// <inheritdoc />
    public string GetDefaultMsgTemplate(ValidationData<TEntity, TProperty> msgData)
        => $"{DefaultErrorMessages._genericErrorMsg} {msgData.PropertyName}.";
}
