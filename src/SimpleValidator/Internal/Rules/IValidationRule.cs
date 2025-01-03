﻿namespace SimpleValidator.Internal.Rules;

/// <summary>
/// Represent rule for validating certain property.
/// </summary>
internal interface IValidationRule
{
    string RuleName { get; }
}

internal interface IValidationRule<TProperty> : IValidationRule
{
    /// <summary>
    /// Method that determines when the rule failed.
    /// </summary>
    /// <param name="propertyValue">Property value that is being validated.</param>
    /// <returns>bool</returns>
    bool FailsWhen(TProperty propertyValue);

    /// <summary>
    /// Method that returns error message when rule failed.
    /// </summary>
    /// <param name="context">data context needed for validation.</param>
    /// <returns>string</returns>
    string GetDefaultMsgTemplate(IValidationContext<TProperty> context);
}

/// <summary>
/// Represent rule for validating certain property.
/// </summary>
/// <typeparam name="TEntity">The main type that is validated.</typeparam>
/// <typeparam name="TProperty">Some property that belongs to the main entity.</typeparam>
internal interface IValidationRule<TEntity, TProperty> : IValidationRule
{
    /// <summary>
    /// Method that determines when the rule failed.
    /// </summary>
    /// <param name="entityValue">Main entity value if the rule needs values for comparison.</param>
    /// <param name="propertyValue">Property value that is being validated.</param>
    /// <returns>bool</returns>
    bool FailsWhen(TEntity entityValue, TProperty propertyValue);

    /// <summary>
    /// Method that returns error message when rule failed.
    /// </summary>
    /// <param name="context">data context needed for validation.</param>
    /// <returns>string</returns>
    string GetDefaultMsgTemplate(IValidationContext<TEntity, TProperty> context);
}