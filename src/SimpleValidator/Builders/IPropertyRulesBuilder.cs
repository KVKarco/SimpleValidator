using SimpleValidator.Rules;
using System.Linq.Expressions;

namespace SimpleValidator.Builders;

/// <summary>
/// Builder for creating rules for TProperty.
/// </summary>
/// <typeparam name="TMainEntity">Type that current abstract validator is build for.</typeparam>
/// <typeparam name="TProperty">Type of property that been validated.</typeparam>
public interface IPropertyRulesBuilder<TMainEntity, TProperty>
{
    /// <summary>
    /// Creates rule from predicate.
    /// </summary>
    /// <param name="predicateExpression">Delegate that determents when the rule fails.</param>
    /// <param name="toShortCircuit">Specifies whether the validation chain stops when the rule fails.</param>
    /// <returns>Error message builder.</returns>
    IErrorMessageBuilder<TMainEntity, TProperty> FailsWhen(
        Expression<Predicate<TProperty>> predicateExpression,
        bool toShortCircuit = false);

    /// <summary>
    /// Creates rule from predicate with comparison values extracted from the main validation entity.
    /// </summary>
    /// <param name="predicateExpression">Delegate that determents when the rule fails.</param>
    /// <param name="toShortCircuit">Specifies whether the validation chain stops when the rule fails.</param>
    /// <returns>Error message builder.</returns>
    IErrorMessageBuilder<TMainEntity, TProperty> FailsWhen(
        Expression<Func<TMainEntity, TProperty, bool>> predicateExpression,
        bool toShortCircuit = false);

    /// <summary>
    /// Adds custom rule to the rules collection.
    /// </summary>
    /// <param name="customRule">Custom AbstractRule.</param>
    /// <param name="toShortCircuit">Specifies whether the validation chain stops when the rule fails.</param>
    IPropertyRulesBuilder<TMainEntity, TProperty> FailsWhen(
        AbstractRule<TMainEntity, TProperty> customRule,
        bool toShortCircuit = false);

    /// <summary>
    /// Attaches nested property validators to the current property validator.
    /// </summary>
    /// <param name="action">Delegate to create nested validators.</param>
    IPropertyRulesBuilder<TMainEntity, TProperty> NestedValidators(
        Action<IGenericValidatorBuilder<TMainEntity, TProperty>> action);
}
