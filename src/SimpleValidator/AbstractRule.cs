﻿using Ardalis.GuardClauses;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Internal.Rules;

namespace SimpleValidator;

/// <summary>
/// Abstract class for creating custom rules.
/// </summary>
/// <typeparam name="TEntity">Type that current abstract validator is build for.</typeparam>
/// <typeparam name="TProperty">Type of property that been validated.</typeparam>
public abstract class AbstractRule<TEntity, TProperty> : IValidationRule<TEntity, TProperty>
{
    /// <summary>
    /// Creates custom rule whit unique rule name.
    /// </summary>
    /// <param name="ruleName">unique rule name.</param>
    protected AbstractRule(string ruleName)
    {
        RuleName = Guard.Against.InternalNullOrWhiteSpace(ruleName);
    }

    /// <summary>
    /// Unique name for the rule.
    /// </summary>
    public string RuleName { get; }

    bool IValidationRule<TEntity, TProperty>.FailsWhen(TEntity entityValue, TProperty propertyValue)
        => FailsWhen(entityValue, propertyValue);

    string IValidationRule<TEntity, TProperty>.GetDefaultMsgTemplate(ValidationData<TEntity, TProperty> msgData)
        => GetDefaultMsgTemplate(msgData);

    /// <summary>
    /// Method that determines when the rule failed.
    /// </summary>
    /// <param name="entityValue">Main entity value if the rule needs values for comparison.</param>
    /// <param name="propertyValue">Property value that is being validated.</param>
    /// <returns>bool</returns>
    public abstract bool FailsWhen(TEntity entityValue, TProperty propertyValue);

    /// <summary>
    /// Method that returns error message when rule failed.
    /// </summary>
    /// <param name="msgData">data for the error message if needed.</param>
    /// <returns>string</returns>
    public abstract string GetDefaultMsgTemplate(ValidationData<TEntity, TProperty> msgData);
}