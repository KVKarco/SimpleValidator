using SimpleValidator.Exceptions;
using SimpleValidator.Internal.Rules.PropertyRules;
using SimpleValidator.Internal.Validators;

namespace SimpleValidator.Internal.Builders;

/// <summary>
/// Interface for managing property validator.
/// </summary>
/// <typeparam name="TEntity">the main type that AbstractValidator is build for.</typeparam>
/// <typeparam name="TProperty">type of property that been validated.</typeparam>
internal interface IPropertyValidatorManager<TEntity, TProperty> :
    IValidatorInfo,
    IValidatorManager<TEntity, TProperty>
{
    /// <summary>
    /// Adds rule to the collection of the rules.
    /// </summary>
    /// <param name="rule">rule that been added</param>
    /// <exception cref="ValidatorArgumentException">If rule is already in the collection</exception>
    void AddRule(IPropertyRule<TEntity, TProperty> rule);

    /// <summary>
    /// Available rules for given property.
    /// </summary>
    PropertyRulesSet<TEntity, TProperty> Rules { get; }
}