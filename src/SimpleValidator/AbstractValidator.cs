using Ardalis.GuardClauses;
using SimpleValidator.Exceptions;
using SimpleValidator.Internal;
using SimpleValidator.Internal.Builders;
using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Internal.Validators;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace SimpleValidator;

/// <summary>
/// Abstract class for creating validators for given type.
/// </summary>
/// <typeparam name="TEntity">Type that been validated.</typeparam>
public abstract class AbstractValidator<TEntity> : IValidatorManager<TEntity, TEntity>, IValidator<TEntity>
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
    /// <exception cref="InvalidSelectorException">When selectorExpression is null or nested property is selected.</exception>
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
    /// <exception cref="InvalidSelectorException">When selectorExpression is null or nested property is selected.</exception>
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
    /// <exception cref="InvalidSelectorException">When selectorExpression is null or nested property is selected.</exception>
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

    /// <summary>
    /// Validates TEntity value.
    /// </summary>
    /// <param name="entity">value of the type that is validated.</param>
    /// <returns>ValidationResult</returns>
    public ValidationResult Validate(TEntity entity)
    {
        ValidationResult result = new();

        if (entity is null)
        {
            result.AddNullWaring($"entity of type {typeof(TEntity)} was null validation stops.");
            return result;
        }

        foreach (var propName in _allowedProps.Select(x => x.Name))
        {
            if (_propertyValidators.TryGetValue(propName, out var validator))
            {
                ValidationRunContext<TEntity, TEntity> context = new(entity, entity, result, propName);
                validator.Validate(context);
            }
        }

        return result;
    }
}
