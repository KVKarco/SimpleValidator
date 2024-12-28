using Ardalis.GuardClauses;
using System.Linq.Expressions;

namespace SimpleValidator.Internal.ExpressionHelpers;

/// <summary>
/// Rewrites selector expressions parameters so predicates can be compared for equality.
/// </summary>
internal sealed class SelectorExpressionRewriter : ExpressionVisitor
{
    private ParameterExpression? _newParam;

    public SelectorExpressionRewriter()
    {
        _newParam = null;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        Guard.Against.Null(node);
        Guard.Against.NullOrWhiteSpace(node.Name);

        _newParam ??= Expression.Parameter(node.Type, "prop");

        return _newParam;
    }
}
