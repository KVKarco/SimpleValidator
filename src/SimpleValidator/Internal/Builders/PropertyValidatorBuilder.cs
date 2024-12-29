using Ardalis.GuardClauses;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Internal.Rules;
using SimpleValidator.Internal.Rules.PropertyRules;
using System.Linq.Expressions;

namespace SimpleValidator.Internal.Builders;

internal sealed class PropertyValidatorBuilder<TEntity, TPropertyValueFrom, TProperty> :
    IPropertyRulesBuilder<TEntity, TProperty>,
    IErrorMessageBuilder<TEntity, TProperty>
{
    private readonly IValidatorManager<TEntity, TPropertyValueFrom> _mainValidator;
    private IPropertyValidatorManager<TEntity, TProperty> _validatorManager;
    private IPropertyRule<TEntity, TProperty>? _currentRuleThatIsBuild;

    public PropertyValidatorBuilder(
        IValidatorManager<TEntity, TPropertyValueFrom> mainValidator,
        IPropertyValidatorManager<TEntity, TProperty> propertyValidator)
    {
        _mainValidator = mainValidator;
        _validatorManager = propertyValidator;
    }

    public IErrorMessageBuilder<TEntity, TProperty> FailsWhen(
        Expression<Predicate<TProperty>> predicateExpression,
        bool toShortCircuit = false)
    {
        Guard.Against.InternalNull(predicateExpression);

        IPropertyRule<TEntity, TProperty> propertyRule = RuleFactory.ForPredicate<TEntity, TProperty>(predicateExpression, toShortCircuit);

        _validatorManager.AddRule(propertyRule);

        _currentRuleThatIsBuild = propertyRule;

        return this;
    }

    public IErrorMessageBuilder<TEntity, TProperty> FailsWhen(
        Expression<Func<TEntity, TProperty, bool>> predicateExpression,
        bool toShortCircuit = false)
    {
        Guard.Against.InternalNull(predicateExpression);

        IPropertyRule<TEntity, TProperty> propertyRule = RuleFactory.ForComparison(predicateExpression, toShortCircuit);

        _validatorManager.AddRule(propertyRule);

        _currentRuleThatIsBuild = propertyRule;

        return this;
    }

    public IPropertyRulesBuilder<TEntity, TProperty> FailsWhen(
        AbstractRule<TEntity, TProperty> customRule,
        bool toShortCircuit = false)
    {
        Guard.Against.InternalNull(customRule);

        IPropertyRule<TEntity, TProperty> propertyRule = RuleFactory.ForCustom(customRule, toShortCircuit);

        _validatorManager.AddRule(propertyRule);

        _currentRuleThatIsBuild = propertyRule;

        return this;
    }

    public IPropertyRulesBuilder<TEntity, TProperty> NestedValidators(
        Action<IGenericValidatorBuilder<TEntity, TProperty>> action)
    {
        Guard.Against.InternalNull(action);

        action(BuilderFactory.ForNestedProperties(_validatorManager));

        return this;
    }

    public IPropertyRulesBuilder<TEntity, TProperty> WithErrorMessage(
        Func<ValidationData<TEntity, TProperty>, string> errorMessageFactory)
    {
        Guard.Against.InternalNull(errorMessageFactory);

        _currentRuleThatIsBuild?.SetErrorMsgFactory(errorMessageFactory);

        _currentRuleThatIsBuild = null;

        return this;
    }

    public IPropertyRulesBuilder<TEntity, TProperty> WithErrorMessage(string errorMessage)
    {
        Guard.Against.InternalNullOrWhiteSpace(errorMessage);

        _currentRuleThatIsBuild?.SetErrorMsg(errorMessage);

        _currentRuleThatIsBuild = null;

        return this;
    }
}
