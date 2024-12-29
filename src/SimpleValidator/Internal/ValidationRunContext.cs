using System.Collections.ObjectModel;

namespace SimpleValidator.Internal;

internal readonly struct ValidationRunContext<TEntity, TPropertyValueFrom>
{
    private readonly string _name;
    private readonly string _displayName;
    private readonly int? _elementIndex;

    public readonly TEntity EntityValue { get; }

    public readonly TPropertyValueFrom PropertyValueFrom { get; }

    public readonly ValidationResult Result { get; }

    public string PropertyName => _elementIndex.HasValue ? $"{_name}[{_elementIndex}]" : _name;

    public string DisplayName => _elementIndex.HasValue ? $"{_displayName}[{_elementIndex}]" : _displayName;

    private ValidationRunContext(TEntity entity, TPropertyValueFrom valueFrom, ValidationResult result, string name, string display, int? index = null)
    {
        EntityValue = entity;
        PropertyValueFrom = valueFrom;
        Result = result;
        _name = name;
        _displayName = display;
        _elementIndex = index;
    }

    public ValidationRunContext(TEntity entityValue, TPropertyValueFrom valueFrom, ValidationResult result, string propertyName)
    {
        EntityValue = entityValue;
        PropertyValueFrom = valueFrom;
        Result = result;
        _name = propertyName;
        _displayName = propertyName;
    }

    public void AttachMustBeNullError() => Result.AddPropertyError(DisplayName, $"{PropertyName} {DefaultErrorMessages._mustBeNull}");

    public void AttachErrors(Collection<string> errors) => Result.AddPropertyErrors(DisplayName, errors);

    public void AttachNullError() => Result.AddPropertyError(DisplayName, $"{PropertyName} {DefaultErrorMessages._notNullMsg}");

    public void AttachNullWorming() => Result.AddNullWaring(DefaultErrorMessages.ReferenceNullWarning(PropertyName, DisplayName));

    public ValidationRunContext<TEntity, TNewPropertyValueFrom> Transform<TNewPropertyValueFrom>(
        TNewPropertyValueFrom valueFrom,
        string name)
    {
        return new ValidationRunContext<TEntity, TNewPropertyValueFrom>(
            EntityValue,
            valueFrom,
            Result,
            name,
           $"{DisplayName}.{name}");
    }

    public ValidationRunContext<TEntity, TPropertyValueFrom> WithIndex(int elementIndex)
    {
        return new ValidationRunContext<TEntity, TPropertyValueFrom>(EntityValue, PropertyValueFrom, Result, PropertyName, DisplayName, elementIndex);
    }
}
