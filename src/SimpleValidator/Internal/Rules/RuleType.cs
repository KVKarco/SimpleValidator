namespace SimpleValidator.Internal.Rules;

/// <summary>
/// What kind of validation rule wrapper(PropertyRule) holds.
/// </summary>
internal enum RuleType
{
    Predicate,
    Comparison,
    Custom,
}
