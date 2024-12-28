using SimpleValidator.Internal;
using SimpleValidator.Rules.PropertyRules;
using SimpleValidator.Validators;
using SimpleValidator.Validators.Assets;

namespace SimpleValidator.Builders.Internal;

/// <summary>
/// Interface for managing property validator.
/// </summary>
/// <typeparam name="TMainEntity">the main type that AbstractValidator is build for.</typeparam>
/// <typeparam name="TProperty">type of property that been validated.</typeparam>
internal interface IPropertyValidatorManager<TMainEntity, TProperty> :
    IValidatorInfo,
    IValidatorManager<TMainEntity, TProperty>
{
    /// <summary>
    /// Adds rule to the collection of the rules.
    /// </summary>
    /// <param name="rule">rule that been added</param>
    /// <exception cref="ValidatorArgumentException">If rule is already in the collection</exception>
    void AddRule(IPropertyRule<TMainEntity, TProperty> rule);

    /// <summary>
    /// Available rules for given property.
    /// </summary>
    PropertyRulesSet<TMainEntity, TProperty> Rules { get; }
}