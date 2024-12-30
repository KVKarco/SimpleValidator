namespace SimpleValidator.Internal.Validators.PropertyValidators;

internal sealed class PropertyValidatorForNullableValueType<TEntity, TPropertyValueFrom, TProperty> :
    BaseValidator<TEntity, TPropertyValueFrom, TProperty>
    where TProperty : struct
{
    private readonly Func<TPropertyValueFrom, TProperty?> _valueGetter;

    public PropertyValidatorForNullableValueType(
        Func<TPropertyValueFrom, TProperty?> propertyValueGetter,
        PropertyOrFieldInfo propertyInfo,
        NullOptions nullOption,
        string? propertyPathPrefix = null)
        : base(propertyInfo, nullOption, propertyPathPrefix)
    {
        _valueGetter = propertyValueGetter;
    }

    public override void Validate(ValidationContext<TEntity, TPropertyValueFrom> context)
    {
        TProperty? nullableValue = _valueGetter(context.PropertyValue);

        if (nullableValue is null)
        {
            if (!Info.IsNullable)
            {
                context.AttachNullWorming(Info.Name);
            }

            if (NullOption == NullOptions.FailsWhenNull)
            {
                context.AttachNullError(Info.Name);
            }
        }
        else
        {
            ValidateCore(context.Transform(nullableValue.Value, Info.Name));
        }
    }
}
