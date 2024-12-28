using Ardalis.GuardClauses;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Rules;
using SimpleValidator.Rules.Assets;
using SimpleValidator.Rules.PropertyRules;
using System.Linq.Expressions;

namespace SimpleValidator.Builders.Internal;

internal sealed class PropertyValidatorBuilder<TMainEntity, TPropertyValueFrom, TProperty> :
    IPropertyRulesBuilder<TMainEntity, TProperty>,
    IErrorMessageBuilder<TMainEntity, TProperty>
{
    private readonly IValidatorManager<TMainEntity, TPropertyValueFrom> _mainValidator;
    private IPropertyValidatorManager<TMainEntity, TProperty> _validatorManager;
    private IPropertyRule<TMainEntity, TProperty>? _currentRuleThatIsBuild;

    public PropertyValidatorBuilder(
        IValidatorManager<TMainEntity, TPropertyValueFrom> mainValidator,
        IPropertyValidatorManager<TMainEntity, TProperty> propertyValidator)
    {
        _mainValidator = mainValidator;
        _validatorManager = propertyValidator;
    }

    public IErrorMessageBuilder<TMainEntity, TProperty> FailsWhen(
        Expression<Predicate<TProperty>> predicateExpression,
        bool toShortCircuit = false)
    {
        Guard.Against.InternalNull(predicateExpression);

        IPropertyRule<TMainEntity, TProperty> propertyRule = RuleFactory.ForPredicate<TMainEntity, TProperty>(predicateExpression, toShortCircuit);

        _validatorManager.AddRule(propertyRule);

        _currentRuleThatIsBuild = propertyRule;

        return this;
    }

    public IErrorMessageBuilder<TMainEntity, TProperty> FailsWhen(
        Expression<Func<TMainEntity, TProperty, bool>> predicateExpression,
        bool toShortCircuit = false)
    {
        Guard.Against.InternalNull(predicateExpression);

        IPropertyRule<TMainEntity, TProperty> propertyRule = RuleFactory.ForComparison(predicateExpression, toShortCircuit);

        _validatorManager.AddRule(propertyRule);

        _currentRuleThatIsBuild = propertyRule;

        return this;
    }

    public IPropertyRulesBuilder<TMainEntity, TProperty> FailsWhen(
        AbstractRule<TMainEntity, TProperty> customRule,
        bool toShortCircuit = false)
    {
        Guard.Against.InternalNull(customRule);

        IPropertyRule<TMainEntity, TProperty> propertyRule = RuleFactory.ForCustom(customRule, toShortCircuit);

        _validatorManager.AddRule(propertyRule);

        _currentRuleThatIsBuild = propertyRule;

        return this;
    }

    public IPropertyRulesBuilder<TMainEntity, TProperty> NestedValidators(
        Action<IGenericValidatorBuilder<TMainEntity, TProperty>> action)
    {
        Guard.Against.InternalNull(action);

        action(BuilderFactory.ForNestedProperties(_validatorManager));

        return this;
    }

    public IPropertyRulesBuilder<TMainEntity, TProperty> WithErrorMessage(
        Func<string, TMainEntity, TProperty, string> errorMessageFactory)
    {
        Guard.Against.InternalNull(errorMessageFactory);

        _currentRuleThatIsBuild?.SetErrorMsgFactory(errorMessageFactory);

        _currentRuleThatIsBuild = null;

        return this;
    }

    public IPropertyRulesBuilder<TMainEntity, TProperty> WithErrorMessage(string errorMessage)
    {
        Guard.Against.InternalNullOrWhiteSpace(errorMessage);

        _currentRuleThatIsBuild?.SetErrorMsg(errorMessage);

        _currentRuleThatIsBuild = null;

        return this;
    }
}
