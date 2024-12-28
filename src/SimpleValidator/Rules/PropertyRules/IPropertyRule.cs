﻿using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Rules.PropertyRules;

/// <summary>
/// Wrapper class for validation rule bound to specific Property validator,
/// so rules can be unique.
/// </summary>
internal interface IPropertyRule
{
    /// <summary>
    /// Unique key for comparing and determining if rule exist.
    /// </summary>
    RuleKey Key { get; }

    /// <summary>
    /// When true validation chains stops if the current rule fails.
    /// </summary>
    bool IsShortCircuit { get; }

    /// <summary>
    /// Sets or changes rule default error message.
    /// </summary>
    void SetErrorMsg(string errorMsg);
}

/// <inheritdoc />
internal interface IPropertyRule<TMainEntity, TProperty> : IPropertyRule
{
    /// <summary>
    /// Sets or changes rule default error message.
    /// </summary>
    void SetErrorMsgFactory(Func<string, TMainEntity, TProperty, string> factory);

    /// <summary>
    /// Determents if rule failed if so returns error message.
    /// </summary>
    /// <param name="propName">property name extracted from member info.</param>
    /// <param name="entityValue">value of the main type that current validator is created for.</param>
    /// <param name="propertyValue">property value that is validated.</param>
    /// <param name="errorMsg">error message that is returned if the rule failed.</param>
    bool Failed(string propName, TMainEntity entityValue, TProperty propertyValue, [NotNullWhen(true)] out string? errorMsg);

    /// <summary>
    /// Transforms(updates) current rule to new form so can be copied in another validator.
    /// </summary>
    /// <typeparam name="TNewEntity">validator type where the current rule is copied to.</typeparam>
    /// <param name="path">string missing selector path from the new validator to the old validator.</param>
    IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path);
}
