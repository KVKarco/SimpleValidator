using SimpleValidator.Internal;

namespace SimpleValidator.Internal.Validators;

/// <summary>
/// Interface that represents how property is validated.
/// </summary>
/// <typeparam name="TEntity">the main type that AbstractValidator is build for.</typeparam>
/// <typeparam name="TPropertyValueFrom">type from where property value is extracted.</typeparam>
internal interface IPropertyValidator<TEntity, TPropertyValueFrom> : IValidatorInfo
{
    /// <summary>
    /// Method for running the validation chain for given property.
    /// </summary>
    /// <param name="context">data that validator needs to validate it self.</param>
    void Validate(in ValidationRunContext<TEntity, TPropertyValueFrom> context);
}
