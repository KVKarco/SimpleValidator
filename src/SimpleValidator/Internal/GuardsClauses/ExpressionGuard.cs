using Ardalis.GuardClauses;
using SimpleValidator.Exceptions;
using SimpleValidator.Internal.ExpressionHelpers;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace SimpleValidator.Internal.GuardsClauses;

internal static class ExpressionGuard
{
    /// <summary>
    /// Checks if selector expression is valid contains nested selections of properties.
    /// </summary>
    /// <exception cref="InvalidSelectorException">When expression is not valid.</exception>
    internal static string InvalidPropertySelector<TEntity, TProperty>(
        this IGuardClause guardClause,
        Expression<Func<TEntity, TProperty>>? propertySelector)
    {
        if (propertySelector is null)
        {
            throw new InvalidSelectorException("Null propertySelector was supplied.");
        }

        if (propertySelector.Body is not MemberExpression memberExpression)
        {
            throw new InvalidSelectorException("propertySelector is not valid MemberExpression.");
        }

        if (memberExpression.Expression is not ParameterExpression)
        {
            throw new InvalidSelectorException("Nested selectors are not supported.");
        }

        return memberExpression.Member.Name;
    }

    /// <summary>
    /// Checks if predicate expression contains nullable members.
    /// </summary>
    /// <exception cref="NullableMemberException">When expression contains nullable members.</exception>
    internal static void IncorrectPredicate(
        this IGuardClause guardClause,
        [NotNull] Expression predicateExpression)
    {
        NullabilityMembersChecker predicateValidator = new();
        predicateValidator.Visit(predicateExpression);

        if (!predicateValidator.IsSafeFromNullableMembers)
        {
            throw new NullableMemberException($"Predicate {predicateExpression} is not valid, nullable members detected.");
        }
    }
}
