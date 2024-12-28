using Ardalis.GuardClauses;
using SimpleValidator.Internal;
using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Validators;
using System.Linq.Expressions;

namespace SimpleValidator.Builders.Internal;

internal sealed class GenericValidatorBuilder<TMainEntity, TPropertyValueFrom> :
    IGenericValidatorBuilder<TMainEntity, TPropertyValueFrom>
{
    private readonly AvailablePropsForValidating _allowedProps;
    private readonly IPropertyValidatorManager<TMainEntity, TPropertyValueFrom> _manager;

    public GenericValidatorBuilder(IPropertyValidatorManager<TMainEntity, TPropertyValueFrom> manager)
    {
        _allowedProps = TypeAvailablePropsCache.GetOrAdd(typeof(TPropertyValueFrom));
        _manager = manager;
    }

    public IPropertyRulesBuilder<TMainEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty>> selectorExpression)
        where TProperty : struct
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_manager.TryGetPropertyValidator(info.Name, out IPropertyValidator<TMainEntity, TPropertyValueFrom>? propertyValidator))
        {
            return BuilderFactory.ForProperty(_manager, (IPropertyValidatorManager<TMainEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(_manager, selectorExpression, info, _manager.PropertyPath);
    }

    public IPropertyRulesBuilder<TMainEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression,
        NullOptions nullOption = NullOptions.Default)
        where TProperty : struct
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_manager.TryGetPropertyValidator(info.Name, out IPropertyValidator<TMainEntity, TPropertyValueFrom>? propertyValidator))
        {
            return BuilderFactory.ForProperty(_manager, (IPropertyValidatorManager<TMainEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(_manager, selectorExpression, info, nullOption, _manager.PropertyPath);
    }

    public IPropertyRulesBuilder<TMainEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression,
        NullOptions nullOption = NullOptions.Default)
        where TProperty : class
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_manager.TryGetPropertyValidator(info.Name, out IPropertyValidator<TMainEntity, TPropertyValueFrom>? propertyValidator))
        {
            return BuilderFactory.ForProperty(_manager, (IPropertyValidatorManager<TMainEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(_manager, selectorExpression, info, nullOption, _manager.PropertyPath);
    }
}
