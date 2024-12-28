namespace SimpleValidator.Internal;

internal static class DefaultErrorMessages
{
    internal const string _genericErrorMsg = "The specified condition was not met for ";
    internal const string _notNullMsg = "cant be null.";
    internal const string _mustBeNull = "must be null.";

    internal static string ReferenceNullWarning(string propertyName, string propertyPath)
        => $"Property {propertyName}, with path: {propertyPath} its not nullable reference type but a null value was provided.";
}
