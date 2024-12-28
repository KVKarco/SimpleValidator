namespace SimpleValidator.Internal;

internal sealed class PropertyOrFieldInfo
{
    public PropertyOrFieldInfo(
        in string propName,
        Type propType,
        Type belongsToType,
        in bool isNullable)
    {
        Name = propName;
        Type = propType;
        BelongsTo = belongsToType;
        IsNullable = isNullable;
    }

    internal string Name { get; }

    internal Type Type { get; }

    internal Type BelongsTo { get; }

    internal bool IsNullable { get; }
}
