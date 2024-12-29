using SimpleValidator.Internal;
using SimpleValidator.Internal.Validators;

namespace SimpleValidator.Internal.Validators.CollectionValidators;

internal sealed class CollectionValidatorForValueTypes<TEntity, TElementValuesFrom, TElement> :
    BaseValidator<TEntity, TElementValuesFrom, TElement>
    where TElementValuesFrom : IEnumerable<TElement>
    where TElement : struct
{
    public CollectionValidatorForValueTypes(PropertyOrFieldInfo propertyInfo, string? propertyPathPrefix = null)
        : base(propertyInfo, NullOptions.Default, propertyPathPrefix)
    {
    }

    public override void Validate(in ValidationRunContext<TEntity, TElementValuesFrom> context)
    {
        TElement[] elements = context.PropertyValueFrom.ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            ValidateCore(context.WithIndex(i), elements[i]);
        }
    }
}
