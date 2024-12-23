namespace SimpleValidator.Internal.ExpressionHelpers;

using Ardalis.GuardClauses;
using System.Linq.Expressions;

internal sealed class SelectorExpressionRewriter : ExpressionVisitor
{
    private ParameterExpression? newParam;

    public SelectorExpressionRewriter()
    {
        this.newParam = null;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        Guard.Against.Null(node);
        Guard.Against.NullOrWhiteSpace(node.Name);

        this.newParam ??= Expression.Parameter(node.Type, "prop");

        return this.newParam;
    }
}
