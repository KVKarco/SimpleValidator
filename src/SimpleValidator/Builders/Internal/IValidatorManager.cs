using SimpleValidator.Validators;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Builders.Internal;

internal interface IValidatorManager<TMainEntity, TPropertyValueFrom>
{
    Dictionary<string, IPropertyValidator<TMainEntity, TPropertyValueFrom>> PropertyValidators { get; }

    bool TryGetPropertyValidator(string propertyName, [NotNullWhen(true)] out IPropertyValidator<TMainEntity, TPropertyValueFrom>? propertyValidator);

    void AddOrReplacePropertyValidator(IPropertyValidator<TMainEntity, TPropertyValueFrom> nestedPropertyValidator);
}
