namespace SimpleValidator.Rules.Assets;

/// <summary>
/// What kind of validation rule wrapper(PropertyRule) holds.
/// </summary>
internal enum RuleType
{
    Predicate,
    Comparison,
    Custom,
}
