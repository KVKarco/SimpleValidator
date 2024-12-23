namespace SimpleValidator.Internal.GuardsClauses;

using Ardalis.GuardClauses;
using SimpleValidator.Internal.ExpressionHelpers;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

internal static class ExpressionGuard
{
    internal static string InvalidPropertySelector<TEntity, TProperty>(
        this IGuardClause guardClause,
        Expression<Func<TEntity, TProperty>>? propertySelector)
    {
        if (propertySelector is null)
        {
            throw new ValidatorArgumentException("Null propertySelector was supplied.");
        }

        if (propertySelector.Body is not MemberExpression memberExpression)
        {
            throw new ValidatorArgumentException("propertySelector is not valid MemberExpression.");
        }

        if (memberExpression.Expression is not ParameterExpression)
        {
            throw new ValidatorArgumentException("Nested selectors are not supported.");
        }

        return memberExpression.Member.Name;
    }

    internal static void IncorrectPredicate(
        this IGuardClause guardClause,
        [NotNull] Expression predicateExpression)
    {
        NullabilityMembersChecker predicateValidator = new NullabilityMembersChecker();
        predicateValidator.Visit(predicateExpression);

        if (!predicateValidator.IsSafeFromNullableMembers)
        {
            throw new ValidatorArgumentException($"Predicate {predicateExpression.ToString()} is not valid, nullable members detected.");
        }
    }
}
