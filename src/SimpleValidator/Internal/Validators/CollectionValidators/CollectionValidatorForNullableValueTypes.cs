using SimpleValidator.Internal;
using SimpleValidator.Internal.Validators;

namespace SimpleValidator.Internal.Validators.CollectionValidators;

internal sealed class CollectionValidatorForNullableValueTypes<TEntity, TElementValuesFrom, TElement> :
    BaseValidator<TEntity, TElementValuesFrom, TElement>
    where TElementValuesFrom : IEnumerable<TElement?>
    where TElement : struct
{
    public CollectionValidatorForNullableValueTypes(PropertyOrFieldInfo propertyInfo, NullOptions nullOption, string? propertyPathPrefix = null)
        : base(propertyInfo, nullOption, propertyPathPrefix)
    {
    }

    public override void Validate(in ValidationRunContext<TEntity, TElementValuesFrom> context)
    {
        TElement?[] elements = context.PropertyValueFrom.ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            var elementContext = context.WithIndex(i);

            TElement? element = elements[i];
            if (!element.HasValue)
            {
                if (!Info.IsNullable)
                {
                    elementContext.AttachNullWorming();
                }

                if (NullOption == NullOptions.FailsWhenNull)
                {
                    elementContext.AttachNullError();
                }
            }
            else
            {
                ValidateCore(elementContext, element.Value);
            }
        }
    }
}
