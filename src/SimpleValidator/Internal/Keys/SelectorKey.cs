namespace SimpleValidator.Internal.Keys;

internal readonly record struct SelectorKey
{
    public SelectorKey(Type inputType, Type propertyType, string displayName)
    {
        this.InputType = inputType;
        this.ReturnType = propertyType;
        this.PropertyPath = displayName;
    }

    internal readonly Type InputType { get; }
    internal readonly Type ReturnType { get; }
    internal readonly string PropertyPath { get; }
}
