using SimpleValidator.Internal;
using SimpleValidator.Internal.Validators;

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

    public override void Validate(in ValidationRunContext<TEntity, TPropertyValueFrom> context)
    {
        TProperty propertyValue = _valueGetter(context.PropertyValueFrom);
        ValidateCore(context, propertyValue);
    }
}
