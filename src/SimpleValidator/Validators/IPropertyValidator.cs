using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Validators;

/// <summary>
/// Interface that represents how property is validated.
/// </summary>
/// <typeparam name="TMainEntity">the main type that AbstractValidator is build for.</typeparam>
/// <typeparam name="TPropertyValueFrom">type from where property value is extracted.</typeparam>
internal interface IPropertyValidator<TMainEntity, TPropertyValueFrom> : IValidatorInfo
{
    /// <summary>
    /// Method for running the validation chain for given property.
    /// </summary>
    /// <param name="entityValue">main type value</param>
    /// <param name="propertyValueFrom">parent validator type value</param>
    /// <param name="result">validation result</param>
    /// <param name="elementIndex">if property validator belongs to collection validator</param>
    void Validate(TMainEntity entityValue, TPropertyValueFrom propertyValueFrom, [NotNull] ValidationResult result, int? elementIndex = null);
}
