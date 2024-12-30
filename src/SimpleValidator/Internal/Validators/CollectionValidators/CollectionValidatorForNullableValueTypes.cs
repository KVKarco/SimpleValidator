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

    public override void Validate(ValidationContext<TEntity, TElementValuesFrom> context)
    {
        TElement?[] elements = context.PropertyValue.ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            TElement? element = elements[i];
            if (!element.HasValue)
            {
                if (!Info.IsNullable)
                {
                    context.AttachNullWorming($"{Info.Name}[{i}]");
                }

                if (NullOption == NullOptions.FailsWhenNull)
                {
                    context.AttachNullError($"{Info.Name}[{i}]");
                }
            }
            else
            {
                ValidateCore(context.Transform(element.Value, i));
            }
        }
    }
}
