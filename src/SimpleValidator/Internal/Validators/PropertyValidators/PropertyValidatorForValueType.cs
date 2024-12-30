namespace SimpleValidator.Internal.Validators.PropertyValidators;

internal sealed class PropertyValidatorForValueType<TEntity, TPropertyValueFrom, TProperty> :
    BaseValidator<TEntity, TPropertyValueFrom, TProperty>
    where TProperty : struct
{
    private readonly Func<TPropertyValueFrom, TProperty> _valueGetter;

    public PropertyValidatorForValueType(
        Func<TPropertyValueFrom, TProperty> propertyValueGetter,
        PropertyOrFieldInfo propertyInfo,
        string? propertyPathPrefix = null)
        : base(propertyInfo, NullOptions.Default, propertyPathPrefix)
    {
        _valueGetter = propertyValueGetter;
    }

    public override void Validate(ValidationContext<TEntity, TPropertyValueFrom> context)
    {
        TProperty propertyValue = _valueGetter(context.PropertyValue);

        ValidateCore(context.Transform(propertyValue, Info.Name));
    }
}
