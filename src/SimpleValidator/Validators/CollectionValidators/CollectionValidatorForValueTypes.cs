using SimpleValidator.Internal;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Validators.CollectionValidators;

internal sealed class CollectionValidatorForValueTypes<TMainEntity, TElementValuesFrom, TElement> :
    BaseValidator<TMainEntity, TElementValuesFrom, TElement>
    where TElementValuesFrom : IEnumerable<TElement>
    where TElement : struct
{
    public CollectionValidatorForValueTypes(PropertyOrFieldInfo propertyInfo, string? propertyPathPrefix = null)
        : base(propertyInfo, NullOptions.Default, $"{propertyPathPrefix}[elementIndex]")
    {
    }

    public override void Validate(TMainEntity entityValue, TElementValuesFrom valuesFrom, [NotNull] ValidationResult result, int? elementIndex = null)
    {
        TElement[] elements = valuesFrom.ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            string displayName = ResolveDisplayName(i);
            string name = ResolveName(i);

            ValidateCore(entityValue, elements[i], name, displayName, result, i);
        }
    }
}
