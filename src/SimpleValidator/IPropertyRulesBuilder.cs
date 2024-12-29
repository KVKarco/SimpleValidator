using SimpleValidator.Exceptions;
using System.Linq.Expressions;

namespace SimpleValidator;

/// <summary>
/// Builder for creating rules for TProperty.
/// </summary>
/// <typeparam name="TEntity">Type that current abstract validator is build for.</typeparam>
/// <typeparam name="TProperty">Type of property that been validated.</typeparam>
public interface IPropertyRulesBuilder<TEntity, TProperty>
{
    /// <summary>
    /// Creates rule from predicate.
    /// </summary>
    /// <param name="predicateExpression">Delegate that determents when the rule fails.</param>
    /// <param name="toShortCircuit">Specifies whether the validation chain stops when the rule fails.</param>
    /// <exception cref="DuplicateRuleException">When duplicate rule is added to the rule set</exception>
    /// <exception cref="NullableMemberException">When predicateExpression contains nullable members.</exception>
    /// <returns>Error message builder.</returns>
    IErrorMessageBuilder<TEntity, TProperty> FailsWhen(
        Expression<Predicate<TProperty>> predicateExpression,
        bool toShortCircuit = false);

    /// <summary>
    /// Creates rule from predicate with comparison values extracted from the main validation entity.
    /// </summary>
    /// <param name="predicateExpression">Delegate that determents when the rule fails.</param>
    /// <param name="toShortCircuit">Specifies whether the validation chain stops when the rule fails.</param>
    /// <exception cref="DuplicateRuleException">When duplicate rule is added to the rule set</exception>
    /// <exception cref="NullableMemberException">When predicateExpression contains nullable members.</exception>
    /// <returns>Error message builder.</returns>
    IErrorMessageBuilder<TEntity, TProperty> FailsWhen(
        Expression<Func<TEntity, TProperty, bool>> predicateExpression,
        bool toShortCircuit = false);

    /// <summary>
    /// Adds custom rule to the rules collection.
    /// </summary>
    /// <param name="customRule">Custom AbstractRule.</param>
    /// <param name="toShortCircuit">Specifies whether the validation chain stops when the rule fails.</param>
    /// /// <exception cref="DuplicateRuleException">When duplicate rule is added to the rule set</exception>
    IPropertyRulesBuilder<TEntity, TProperty> FailsWhen(
        AbstractRule<TEntity, TProperty> customRule,
        bool toShortCircuit = false);

    /// <summary>
    /// Attaches nested property validators to the current property validator.
    /// </summary>
    /// <param name="action">Delegate to create nested validators.</param>
    IPropertyRulesBuilder<TEntity, TProperty> NestedValidators(
        Action<IGenericValidatorBuilder<TEntity, TProperty>> action);
}
