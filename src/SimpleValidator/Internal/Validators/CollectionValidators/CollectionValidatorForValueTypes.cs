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

    public override void Validate(ValidationContext<TEntity, TElementValuesFrom> context)
    {
        TElement[] elements = context.PropertyValue.ToArray();

        for (int i = 0; i < elements.Length; i++)
        {
            ValidateCore(context.Transform(elements[i], i));
        }
    }
}
