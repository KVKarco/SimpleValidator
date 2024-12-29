using SimpleValidator.Internal;

namespace SimpleValidator.Internal.Validators;

/// <summary>
/// Info for the current property validator.
/// </summary>
internal interface IValidatorInfo
{
    /// <summary>
    /// Info for the type that been validated.
    /// </summary>
    PropertyOrFieldInfo Info { get; }

    /// <inheritdoc cref="NullOptions" />
    NullOptions NullOption { get; }

    string PropertyPath { get; }
}
