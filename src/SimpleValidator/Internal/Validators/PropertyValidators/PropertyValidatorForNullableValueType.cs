using SimpleValidator.Internal;
using SimpleValidator.Internal.Validators;

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

    public override void Validate(in ValidationRunContext<TEntity, TPropertyValueFrom> context)
    {
        TProperty? nullableValue = _valueGetter(context.PropertyValueFrom);

        if (nullableValue is null)
        {
            if (!Info.IsNullable)
            {
                context.AttachNullWorming();
            }

            if (NullOption == NullOptions.FailsWhenNull)
            {
                context.AttachNullError();
            }
        }
        else
        {
            ValidateCore(context, nullableValue.Value);
        }
    }
}
