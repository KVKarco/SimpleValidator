using System.Collections.ObjectModel;

namespace SimpleValidator.Internal;

internal readonly struct ValidationContext<TEntity, TProperty> : IValidationContext<TEntity, TProperty>
{
    public ValidationContext(TEntity entityValue, TProperty propertyValue, ValidationResult result)
    {
        EntityValue = entityValue;
        PropertyValue = propertyValue;
        Result = result;
        PropertyName = string.Empty;
        DisplayName = string.Empty;
    }

    private ValidationContext(
        TEntity entityValue,
        TProperty propertyValue,
        ValidationResult result,
        string propertyName,
        string displayName)
    {
        EntityValue = entityValue;
        PropertyValue = propertyValue;
        Result = result;
        PropertyName = propertyName;
        DisplayName = displayName;
    }

    public TEntity EntityValue { get; }

    public TProperty PropertyValue { get; }

    public ValidationResult Result { get; }

    public readonly string PropertyName { get; }

    public readonly string DisplayName { get; }

    internal ValidationContext<TEntity, TNewProperty> Transform<TNewProperty>(TNewProperty propertyValue, string propertyName)
    {
        return new ValidationContext<TEntity, TNewProperty>(
            EntityValue,
            propertyValue,
            Result,
            propertyName,
            DisplayName.Length == 0 ? propertyName : $"{DisplayName}.{propertyName}");
    }

    internal ValidationContext<TEntity, TElement> Transform<TElement>(TElement elementValue, int index)
    {
        return new ValidationContext<TEntity, TElement>(
            EntityValue,
            elementValue,
            Result,
            $"{PropertyName}[{index}]",
            $"{DisplayName}[{index}]");
    }

    internal ValidationContext<TOldEntity, TProperty> Transform<TOldEntity>(TOldEntity entityValue)
    {
        return new ValidationContext<TOldEntity, TProperty>(
            entityValue,
            PropertyValue,
            Result,
            PropertyName,
            DisplayName);
    }

    internal void AttachMustBeNullError()
    => Result.AddPropertyError(DisplayName, $"{PropertyName} {DefaultErrorMessages.MustBeNull}");

    internal void AttachErrors(Collection<string> errors) => Result.AddPropertyErrors(DisplayName, errors);

    internal void AttachNullError(string propertyName)
        => Result.AddPropertyError(
            DisplayName.Length == 0 ? propertyName : $"{DisplayName}.{propertyName}",
            $"{propertyName} {DefaultErrorMessages.NotNullMsg}");

    internal void AttachNullWorming(string propertyName)
        => Result.AddNullWaring(DefaultErrorMessages.ReferenceNullWarning(
            propertyName,
            DisplayName.Length == 0 ? propertyName : $"{DisplayName}.{propertyName}"));
}
