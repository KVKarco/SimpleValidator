using SimpleValidator.Internal;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Validators.CollectionValidators;

internal sealed class CollectionValidatorForReferenceType<TMainEntity, TElementValuesFrom, TElement> :
    BaseValidator<TMainEntity, TElementValuesFrom, TElement>
    where TElementValuesFrom : IEnumerable<TElement?>
    where TElement : class
{
    public CollectionValidatorForReferenceType(PropertyOrFieldInfo propertyInfo, NullOptions nullOption, string? propertyPathPrefix = null)
        : base(propertyInfo, nullOption, $"{propertyPathPrefix}[elementIndex]")
    {
    }

    public override void Validate(TMainEntity entityValue, TElementValuesFrom valuesFrom, [NotNull] ValidationResult result, int? elementIndex = null)
    {
        TElement?[] elements = valuesFrom.ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            string displayName = ResolveDisplayName(i);
            string name = ResolveName(i);

            TElement? element = elements[i];
            if (element is null)
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
                ValidateCore(entityValue, element, name, displayName, result, i);
            }
        }
    }
}
