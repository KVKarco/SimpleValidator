using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.Keys;
using SimpleValidator.Internal.Validators.PropertyValidators;
using System.Linq.Expressions;

namespace SimpleValidator.Internal.Builders;

internal static class BuilderFactory
{
    public static PropertyValidatorBuilder<TEntity, TPropertyValueFrom, TProperty> ForProperty<TEntity, TPropertyValueFrom, TProperty>(
        IValidatorManager<TEntity, TPropertyValueFrom> manager,
        IPropertyValidatorManager<TEntity, TProperty> propertyValidator)
    {
        return new(manager, propertyValidator);
    }

    public static PropertyValidatorBuilder<TEntity, TPropertyValueFrom, TProperty> ForProperty<TEntity, TPropertyValueFrom, TProperty>(
    IValidatorManager<TEntity, TPropertyValueFrom> manager,
    Expression<Func<TPropertyValueFrom, TProperty>> selectorExpression,
    PropertyOrFieldInfo info,
    string? propertyPathPrefix = null)
    where TProperty : struct
    {
        SelectorKey key = new(typeof(TPropertyValueFrom), info.Type, propertyPathPrefix == null ? info.Name : $"{propertyPathPrefix}.{info.Name}");
        Func<TPropertyValueFrom, TProperty> valueGetter = SelectorsCache.GetOrAdd(key, selectorExpression);

        PropertyValidatorForValueType<TEntity, TPropertyValueFrom, TProperty> propertyValidator =
            new(valueGetter, info, propertyPathPrefix);

        manager.AddOrReplacePropertyValidator(propertyValidator);

        return new(manager, propertyValidator);
    }

    public static PropertyValidatorBuilder<TEntity, TPropertyValueFrom, TProperty> ForProperty<TEntity, TPropertyValueFrom, TProperty>(
        IValidatorManager<TEntity, TPropertyValueFrom> manager,
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression,
        PropertyOrFieldInfo info,
        NullOptions option,
        string? propertyPathPrefix = null)
        where TProperty : struct
    {
        SelectorKey key = new(typeof(TPropertyValueFrom), info.Type, propertyPathPrefix == null ? info.Name : $"{propertyPathPrefix}.{info.Name}");
        Func<TPropertyValueFrom, TProperty?> valueGetter = SelectorsCache.GetOrAdd(key, selectorExpression);

        PropertyValidatorForNullableValueType<TEntity, TPropertyValueFrom, TProperty> propertyValidator = new(
            valueGetter,
            info,
            option == NullOptions.Default ? NullOptions.FailsWhenNull : option,
            propertyPathPrefix);

        manager.AddOrReplacePropertyValidator(propertyValidator);

        return new(manager, propertyValidator);
    }

    public static PropertyValidatorBuilder<TEntity, TPropertyValueFrom, TProperty> ForProperty<TEntity, TPropertyValueFrom, TProperty>(
        IValidatorManager<TEntity, TPropertyValueFrom> manager,
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression,
        in PropertyOrFieldInfo info,
        NullOptions option,
        string? propertyPathPrefix = null)
        where TProperty : class
    {
        SelectorKey key = new(typeof(TPropertyValueFrom), info.Type, propertyPathPrefix == null ? info.Name : $"{propertyPathPrefix}.{info.Name}");
        Func<TPropertyValueFrom, TProperty?> valueGetter = SelectorsCache.GetOrAdd(key, selectorExpression);

        PropertyValidatorForReferenceType<TEntity, TPropertyValueFrom, TProperty> propertyValidator = new(
            valueGetter,
            info,
            info.IsNullable ? option == NullOptions.Default ? NullOptions.FailsWhenNull : option : NullOptions.Default,
            propertyPathPrefix);

        manager.AddOrReplacePropertyValidator(propertyValidator);

        return new(manager, propertyValidator);
    }

    public static IGenericValidatorBuilder<TEntity, TPropertyValueFrom> ForNestedProperties<TEntity, TPropertyValueFrom>(
        IPropertyValidatorManager<TEntity, TPropertyValueFrom> manager)
    {
        return new InlineValidatorBuilder<TEntity, TPropertyValueFrom>(manager);
    }
}
