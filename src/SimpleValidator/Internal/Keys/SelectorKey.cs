namespace SimpleValidator.Internal.Keys;

internal readonly record struct SelectorKey
{
    public SelectorKey(Type inputType, Type propertyType, string displayName)
    {
        InputType = inputType;
        ReturnType = propertyType;
        PropertyPath = displayName;
    }

    internal readonly Type InputType { get; }
    internal readonly Type ReturnType { get; }
    internal readonly string PropertyPath { get; }
}
