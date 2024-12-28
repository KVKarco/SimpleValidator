using SimpleValidator.Internal;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Validators.PropertyValidators;

internal sealed class PropertyValidatorForValueType<TMainEntity, TPropertyValueFrom, TProperty> :
    BaseValidator<TMainEntity, TPropertyValueFrom, TProperty>
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

    public override void Validate(TMainEntity entityValue, TPropertyValueFrom valueFrom, [NotNull] ValidationResult result, int? elementIndex = null)
    {
        string name = ResolveName(elementIndex);
        string displayName = ResolveDisplayName(elementIndex);

        TProperty propertyValue = _valueGetter(valueFrom);
        ValidateCore(entityValue, propertyValue, name, displayName, result, elementIndex);
    }
}
