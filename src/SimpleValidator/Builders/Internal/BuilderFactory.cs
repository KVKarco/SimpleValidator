using SimpleValidator.Internal;
using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.Keys;
using SimpleValidator.Validators.PropertyValidators;
using System.Linq.Expressions;

namespace SimpleValidator.Builders.Internal;

internal static class BuilderFactory
{
    public static PropertyValidatorBuilder<TMainEntity, TPropertyValueFrom, TProperty> ForProperty<TMainEntity, TPropertyValueFrom, TProperty>(
        IValidatorManager<TMainEntity, TPropertyValueFrom> manager,
        IPropertyValidatorManager<TMainEntity, TProperty> propertyValidator)
    {
        return new(manager, propertyValidator);
    }

    public static PropertyValidatorBuilder<TMainEntity, TPropertyValueFrom, TProperty> ForProperty<TMainEntity, TPropertyValueFrom, TProperty>(
    IValidatorManager<TMainEntity, TPropertyValueFrom> manager,
    Expression<Func<TPropertyValueFrom, TProperty>> selectorExpression,
    PropertyOrFieldInfo info,
    string? propertyPathPrefix = null)
    where TProperty : struct
    {
        SelectorKey key = new(typeof(TPropertyValueFrom), info.Type, propertyPathPrefix == null ? info.Name : $"{propertyPathPrefix}.{info.Name}");
        Func<TPropertyValueFrom, TProperty> valueGetter = SelectorsCache.GetOrAdd(key, selectorExpression);

        PropertyValidatorForValueType<TMainEntity, TPropertyValueFrom, TProperty> propertyValidator =
            new(valueGetter, info, propertyPathPrefix);

        manager.AddOrReplacePropertyValidator(propertyValidator);

        return new(manager, propertyValidator);
    }

    public static PropertyValidatorBuilder<TMainEntity, TPropertyValueFrom, TProperty> ForProperty<TMainEntity, TPropertyValueFrom, TProperty>(
        IValidatorManager<TMainEntity, TPropertyValueFrom> manager,
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression,
        PropertyOrFieldInfo info,
        NullOptions option,
        string? propertyPathPrefix = null)
        where TProperty : struct
    {
        SelectorKey key = new(typeof(TPropertyValueFrom), info.Type, propertyPathPrefix == null ? info.Name : $"{propertyPathPrefix}.{info.Name}");
        Func<TPropertyValueFrom, TProperty?> valueGetter = SelectorsCache.GetOrAdd(key, selectorExpression);

        PropertyValidatorForNullableValueType<TMainEntity, TPropertyValueFrom, TProperty> propertyValidator = new(
            valueGetter,
            info,
            option == NullOptions.Default ? NullOptions.FailsWhenNull : option,
            propertyPathPrefix);

        manager.AddOrReplacePropertyValidator(propertyValidator);

        return new(manager, propertyValidator);
    }

    public static PropertyValidatorBuilder<TMainEntity, TPropertyValueFrom, TProperty> ForProperty<TMainEntity, TPropertyValueFrom, TProperty>(
        IValidatorManager<TMainEntity, TPropertyValueFrom> manager,
        Expression<Func<TPropertyValueFrom, TProperty?>> selectorExpression,
        in PropertyOrFieldInfo info,
        NullOptions option,
        string? propertyPathPrefix = null)
        where TProperty : class
    {
        SelectorKey key = new(typeof(TPropertyValueFrom), info.Type, propertyPathPrefix == null ? info.Name : $"{propertyPathPrefix}.{info.Name}");
        Func<TPropertyValueFrom, TProperty?> valueGetter = SelectorsCache.GetOrAdd(key, selectorExpression);

        PropertyValidatorForReferenceType<TMainEntity, TPropertyValueFrom, TProperty> propertyValidator = new(
            valueGetter,
            info,
            info.IsNullable ? (option == NullOptions.Default ? NullOptions.FailsWhenNull : option) : NullOptions.Default,
            propertyPathPrefix);

        manager.AddOrReplacePropertyValidator(propertyValidator);

        return new(manager, propertyValidator);
    }

    public static IGenericValidatorBuilder<TMainEntity, TPropertyValueFrom> ForNestedProperties<TMainEntity, TPropertyValueFrom>(
        IPropertyValidatorManager<TMainEntity, TPropertyValueFrom> manager)
    {
        return new GenericValidatorBuilder<TMainEntity, TPropertyValueFrom>(manager);
    }
}
