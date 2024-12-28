using SimpleValidator.Internal;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Validators.PropertyValidators;

internal sealed class PropertyValidatorForNullableValueType<TMainEntity, TPropertyValueFrom, TProperty> :
    BaseValidator<TMainEntity, TPropertyValueFrom, TProperty>
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

    public override void Validate(TMainEntity entityValue, TPropertyValueFrom valueFrom, [NotNull] ValidationResult result, int? elementIndex = null)
    {
        string name = ResolveName(elementIndex);
        string displayName = ResolveDisplayName(elementIndex);

        TProperty? nullableValue = _valueGetter(valueFrom);

        if (nullableValue is null)
        {
            if (!Info.IsNullable)
            {
                result.AddNullWaring(DefaultErrorMessages.ReferenceNullWarning(name, displayName));
            }

            if (NullOption == NullOptions.FailsWhenNull)
            {
                result.AddPropertyError(displayName, $"{name} {DefaultErrorMessages._notNullMsg}");
            }
        }
        else
        {
            ValidateCore(entityValue, nullableValue.Value, name, displayName, result, elementIndex);
        }
    }
}
