using SimpleValidator.Internal.Validators;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Internal.Builders;

internal interface IValidatorManager<TEntity, TPropertyValueFrom>
{
    Dictionary<string, IPropertyValidator<TEntity, TPropertyValueFrom>> PropertyValidators { get; }

    bool TryGetPropertyValidator(string propertyName, [NotNullWhen(true)] out IPropertyValidator<TEntity, TPropertyValueFrom>? propertyValidator);

    void AddOrReplacePropertyValidator(IPropertyValidator<TEntity, TPropertyValueFrom> nestedPropertyValidator);
}
