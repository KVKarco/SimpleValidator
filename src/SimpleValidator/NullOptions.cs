namespace SimpleValidator;

/// <summary>
/// What happens if a null value is received.
/// </summary>
public enum NullOptions
{
    /// <summary>
    /// Can be selected for nullable value types and nullable reference types.
    /// The property must be null, if is not null error message is added.
    /// and validation chain stops.
    /// </summary>
    MustBeNull,

    /// <summary>
    /// Can be selected for nullable value types and nullable reference types.
    /// The property cant be null, if it is a null error message is added,
    /// and validation chain stops,
    /// if not null validation chain continues.
    /// </summary>
    FailsWhenNull,

    /// <summary>
    /// Can be selected for nullable value types and nullable reference types.
    /// The property cant be null, if it is a null validation chain stops,
    /// without error message, for optional properties.
    /// </summary>
    NotValidateWhenNull,

    /// <summary>
    /// The property is not nullable struct.
    /// The property is not nullable reference type.
    /// The property is nullable reference type FailsWhenNull is selected.
    /// The property is nullable value type FailsWhenNull is selected.
    /// </summary>
    Default
}
