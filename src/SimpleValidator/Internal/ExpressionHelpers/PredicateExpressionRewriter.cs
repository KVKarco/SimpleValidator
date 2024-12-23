namespace SimpleValidator.Internal.ExpressionHelpers;

using Ardalis.GuardClauses;
using System.Linq.Expressions;

internal sealed class PredicateExpressionRewriter : ExpressionVisitor
{
#pragma warning disable SA1010 // Opening square brackets should be spaced correctly
    private readonly string[] propertyName = ["param1", "param2", "param3", "param4", "param5", "param6", "param7", "param8", "param9"];
#pragma warning restore SA1010 // Opening square brackets should be spaced correctly
    private readonly Dictionary<string, ParameterExpression> propertyMap = new Dictionary<string, ParameterExpression>();
    private int paramToUse;

    protected override Expression VisitParameter(ParameterExpression node)
    {
        Guard.Against.Null(node);
        Guard.Against.NullOrWhiteSpace(node.Name);

        if (this.propertyMap.TryGetValue(node.Name, out ParameterExpression? value))
        {
            return value;
        }
        else
        {
            ParameterExpression newParam = Expression.Parameter(node.Type, this.propertyName[this.paramToUse]);
            this.propertyMap.Add(node.Name, newParam);
            this.paramToUse++;
            return newParam;
        }
    }
}
