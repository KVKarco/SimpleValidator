using System.Linq.Expressions;

namespace SimpleValidator.Builders;

/// <summary>
/// Builder for nested property rules.
/// </summary>
/// <typeparam name="TMainEntity">Type that current abstract validator is build for.</typeparam>
/// <typeparam name="TPropertyValueFrom">Type of property that nested validators are build for.</typeparam>
public interface IGenericValidatorBuilder<TMainEntity, TPropertyValueFrom>
{
    /// <summary>
    /// Create Rules builder for the current not nullable value type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    IPropertyRulesBuilder<TMainEntity, TProperty> ValidationsFor<TProperty>(
    Expression<Func<TPropertyValueFrom, TProperty>> selectorExpression)
    where TProperty : struct;

    /// <summary>
    /// Create Rules builder for the current nullable value type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    /// <param name="nullOption">Nullability option</param>
    /// <seealso cref="NullOptions"/>
    IPropertyRulesBuilder<TMainEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression, NullOptions nullOption = NullOptions.Default)
        where TProperty : struct;

    /// <summary>
    /// Create Rules builder for the current reference type property.
    /// </summary>
    /// <typeparam name="TProperty">Type of property that's been validated.</typeparam>
    /// <param name="selectorExpression">Expression for selecting property for validation.</param>
    /// <param name="nullOption">Nullability option</param>
    /// <seealso cref="NullOptions"/>
    public IPropertyRulesBuilder<TMainEntity, TProperty> ValidationsFor<TProperty>(
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression, NullOptions nullOption = NullOptions.Default)
        where TProperty : class;
}
