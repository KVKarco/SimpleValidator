namespace SimpleValidator.Internal.Rules;

/// <summary>
/// Validation rule from predicate.
/// </summary>
internal sealed class PredicateRule<TProperty> : IValidationRule<TProperty>
{
    private readonly Predicate<TProperty> _predicate;

    public PredicateRule(Predicate<TProperty> predicate, string ruleName)
    {
        _predicate = predicate;
        RuleName = ruleName;
    }

    public string RuleName { get; }

    /// <inheritdoc />
    public bool FailsWhen(TProperty propertyValue) => _predicate(propertyValue);

    /// <inheritdoc />
    public string GetDefaultMsgTemplate(IValidationContext<TProperty> context)
        => $"{DefaultErrorMessages.GenericErrorMsg} {context.PropertyName}.";
}
