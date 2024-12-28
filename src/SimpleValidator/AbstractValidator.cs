using Ardalis.GuardClauses;
using SimpleValidator.Builders;
using SimpleValidator.Builders.Internal;
using SimpleValidator.Internal;
using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Validators;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace SimpleValidator;

/// <summary>
/// Abstract class for creating validators for given type.
/// </summary>
/// <typeparam name="TEntity">Type that been validated.</typeparam>
public abstract class AbstractValidator<TEntity> : IValidatorManager<TEntity, TEntity>
{
    private readonly AvailablePropsForValidating _allowedProps;
    private readonly Dictionary<string, IPropertyValidator<TEntity, TEntity>> _propertyValidators;

    /// <summary>
    /// Creates abstract validator with available props.
    /// </summary>
    protected AbstractValidator()
    {
        _allowedProps = TypeAvailablePropsCache.GetOrAdd(typeof(TEntity));
        _propertyValidators = [];
    }

    Dictionary<string, IPropertyValidator<TEntity, TEntity>> IValidatorManager<TEntity, TEntity>.PropertyValidators => _propertyValidators;

    void IValidatorManager<TEntity, TEntity>.AddOrReplacePropertyValidator(IPropertyValidator<TEntity, TEntity> propertyValidator)
    {
        _propertyValidators[propertyValidator.Info.Name] = propertyValidator;
    }

    bool IValidatorManager<TEntity, TEntity>.TryGetPropertyValidator(
        string propertyName,
        [NotNullWhen(true)] out IPropertyValidator<TEntity, TEntity>? propertyValidator)
    {
        return _propertyValidators.TryGetValue(propertyName, out propertyValidator);
    }

    /// <summary>
    /// Create Rules builder for the current not nullable value type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    protected IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TEntity, TProperty>> selectorExpression)
        where TProperty : struct
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_propertyValidators.TryGetValue(info.Name, out IPropertyValidator<TEntity, TEntity>? propertyValidator))
        {
            return BuilderFactory.ForProperty(this, (IPropertyValidatorManager<TEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(this, selectorExpression, info);
    }

    /// <summary>
    /// Create Rules builder for the current nullable value type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    /// <param name="nullOption">Nullability option</param>
    /// <seealso cref="NullOptions"/>
    protected IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TEntity, TProperty?>> selectorExpression, NullOptions nullOption = NullOptions.Default)
        where TProperty : struct
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_propertyValidators.TryGetValue(info.Name, out IPropertyValidator<TEntity, TEntity>? propertyValidator))
        {
            return BuilderFactory.ForProperty(this, (IPropertyValidatorManager<TEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(this, selectorExpression, info, nullOption);
    }

    /// <summary>
    /// Create Rules builder for the current reference type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    /// <param name="nullOption">Nullability option</param>
    /// <seealso cref="NullOptions"/>
    protected IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TEntity, TProperty?>> selectorExpression, NullOptions nullOption = NullOptions.Default)
        where TProperty : class
    {
        string propertyName = Guard.Against.InvalidPropertySelector(selectorExpression);
        PropertyOrFieldInfo info = _allowedProps.GetInfoOrThrow(propertyName);

        if (_propertyValidators.TryGetValue(info.Name, out IPropertyValidator<TEntity, TEntity>? propertyValidator))
        {
            return BuilderFactory.ForProperty(this, (IPropertyValidatorManager<TEntity, TProperty>)propertyValidator);
        }

        return BuilderFactory.ForProperty(this, selectorExpression, info, nullOption);
    }
}
