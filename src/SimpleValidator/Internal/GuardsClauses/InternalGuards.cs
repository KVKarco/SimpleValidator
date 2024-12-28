using Ardalis.GuardClauses;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SimpleValidator.Internal.GuardsClauses;

internal static class InternalGuards
{
    private static Func<ValidatorArgumentException> Null(string parameterName)
    {
        return () => new ValidatorArgumentException($"Required input {parameterName} was null.");
    }

    private static Func<ValidatorArgumentException> NullOrEmptyExceptionCreator(string parameterName)
    {
        return () => new ValidatorArgumentException($"Required input {parameterName} was null or empty.");
    }

    internal static Type UnsupportedType(this IGuardClause guardClause, [NotNull] Type type)
    {
        if (type.AssemblyQualifiedName == null)
        {
            throw new ValidatorArgumentException($"Type: {type} is not supported for validation.");
        }

        return type;
    }

    internal static string InternalNullOrWhiteSpace(
    this IGuardClause guardClause,
    [NotNull][ValidatedNotNull] string input,
    [CallerArgumentExpression(nameof(input))] string? parameterName = null)
    {
        return Guard.Against.NullOrWhiteSpace(input, parameterName, null, NullOrEmptyExceptionCreator(parameterName!));
    }

    internal static T InternalNull<T>(
        this IGuardClause guardClause,
        [NotNull][ValidatedNotNull] T input,
        [CallerArgumentExpression(nameof(input))] string? parameterName = null)
    {
        return Guard.Against.Null(input, parameterName, null, Null(parameterName!));
    }
}
