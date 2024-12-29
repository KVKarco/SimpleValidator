using SimpleValidator.Exceptions;
using System.Linq.Expressions;

namespace SimpleValidator;

/// <summary>
/// Builder for nested property rules.
/// </summary>
/// <typeparam name="TEntity">Type that current abstract validator is build for.</typeparam>
/// <typeparam name="TPropertyValueFrom">Type of property that nested validators are build for.</typeparam>
public interface IGenericValidatorBuilder<TEntity, TPropertyValueFrom>
{
    /// <summary>
    /// Create Rules builder for the current not nullable value type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    /// <exception cref="InvalidSelectorException">When selectorExpression is null or nested property is selected.</exception>
    IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
    Expression<Func<TPropertyValueFrom, TProperty>> selectorExpression)
    where TProperty : struct;

    /// <summary>
    /// Create Rules builder for the current nullable value type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    /// <param name="nullOption">Nullability option</param>
    /// <seealso cref="NullOptions"/>
    /// <exception cref="InvalidSelectorException">When selectorExpression is null or nested property is selected.</exception>
    IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression, NullOptions nullOption = NullOptions.Default)
        where TProperty : struct;

    /// <summary>
    /// Create Rules builder for the current reference type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    /// <param name="nullOption">Nullability option</param>
    /// <seealso cref="NullOptions"/>
    /// <exception cref="InvalidSelectorException">When selectorExpression is null or nested property is selected.</exception>
    public IPropertyRulesBuilder<TEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression, NullOptions nullOption = NullOptions.Default)
        where TProperty : class;
}
