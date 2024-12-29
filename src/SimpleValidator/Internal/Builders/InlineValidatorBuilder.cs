using Ardalis.GuardClauses;
using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Internal.Validators;
using System.Linq.Expressions;

namespace SimpleValidator.Internal.Builders;

internal sealed class InlineValidatorBuilder<TEntity, TPropertyValueFrom> :
    IGenericValidatorBuilder<TEntity, TPropertyValueFrom>
{
    private readonly AvailablePropsForValidating _allowedProps;
    private readonly IPropertyValidatorManager<TEntity, TPropertyValueFrom> _manager;

    public InlineValidatorBuilder(IPropertyValidatorManager<TEntity, TPropertyValueFrom> manager)
    {
        _allowedProps = TypeAvailablePropsCache.GetOrAdd(typeof(TPropertyValueFrom));
        _manager = manager;
    }

    public IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty>> selectorExpression)
        where TProperty : struct
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_manager.TryGetPropertyValidator(info.Name, out IPropertyValidator<TEntity, TPropertyValueFrom>? propertyValidator))
        {
            return BuilderFactory.ForProperty(_manager, (IPropertyValidatorManager<TEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(_manager, selectorExpression, info, _manager.PropertyPath);
    }

    public IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression,
        NullOptions nullOption = NullOptions.Default)
        where TProperty : struct
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_manager.TryGetPropertyValidator(info.Name, out IPropertyValidator<TEntity, TPropertyValueFrom>? propertyValidator))
        {
            return BuilderFactory.ForProperty(_manager, (IPropertyValidatorManager<TEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(_manager, selectorExpression, info, nullOption, _manager.PropertyPath);
    }

    public IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression,
        NullOptions nullOption = NullOptions.Default)
        where TProperty : class
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_manager.TryGetPropertyValidator(info.Name, out IPropertyValidator<TEntity, TPropertyValueFrom>? propertyValidator))
        {
            return BuilderFactory.ForProperty(_manager, (IPropertyValidatorManager<TEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(_manager, selectorExpression, info, nullOption, _manager.PropertyPath);
    }
}
