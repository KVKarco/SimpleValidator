namespace SimpleValidator.Internal;

internal static class DefaultErrorMessages
{
    public const string GenericErrorMsg = "The specified condition was not met for ";
    public const string NotNullMsg = "cant be null.";
    public const string MustBeNull = "must be null.";

    public static string MustBeEqual<TProperty>(string propertyName, TProperty value)
        => $"{propertyName} must be equal to {value?.ToString() ?? "null"}.";

    public static string CantBeEqual<TProperty>(string parameterName, TProperty value)
        => $"{parameterName} cant be equal to {value?.ToString() ?? "null"}.";

    public static string NotEmptyString(string parameterName)
        => $"{parameterName} cant be empty.";

    public static string NotEmptyCollection(string parameterName)
        => $"{parameterName} must contain items.";

    public static string ReferenceNullWarning(string propertyName, string propertyPath)
        => $"Property {propertyName}, with path: {propertyPath} its not nullable reference type but a null value was provided.";
}
