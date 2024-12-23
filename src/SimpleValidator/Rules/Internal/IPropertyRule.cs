namespace SimpleValidator.Rules.Internal;

using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

internal interface IPropertyRule
{
    RuleKey Key { get; }

    bool IsShortCircuit { get; }

    void SetErrorMsg(string errorMsg);
}

/// <summary>
/// Wrapper class for validation rule bound to specific Property validator,
/// so rules can be unique.
/// </summary>
internal interface IPropertyRule<TMainEntity, TProperty> : IPropertyRule
{
    void SetErrorMsgFactory(Func<string, TMainEntity, TProperty, string> factory);

    bool Failed(string propName, TMainEntity entityValue, TProperty propertyValue, [NotNullWhen(true)] out string? errorMsg);
}
