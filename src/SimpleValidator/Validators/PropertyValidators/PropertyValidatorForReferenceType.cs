using SimpleValidator.Internal;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Validators.PropertyValidators;

internal sealed class PropertyValidatorForReferenceType<TMainEntity, TPropertyValueFrom, TProperty> :
    BaseValidator<TMainEntity, TPropertyValueFrom, TProperty>
    where TProperty : class
{
    private readonly Func<TPropertyValueFrom, TProperty?> _valueGetter;

    public PropertyValidatorForReferenceType(
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

        TProperty? propertyValue = _valueGetter(valueFrom);

        if (propertyValue is null)
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
            ValidateCore(entityValue, propertyValue, name, displayName, result, elementIndex);
        }
    }
}
