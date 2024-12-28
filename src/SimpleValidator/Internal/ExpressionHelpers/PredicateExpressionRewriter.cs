using Ardalis.GuardClauses;
using System.Linq.Expressions;

namespace SimpleValidator.Internal.ExpressionHelpers;

/// <summary>
/// Rewrites predicate expressions parameters so predicates can be compared for equality.
/// </summary>
internal sealed class PredicateExpressionRewriter : ExpressionVisitor
{
    private readonly string[] _propertyName = ["param1", "param2", "param3", "param4", "param5", "param6", "param7", "param8", "param9"];
    private readonly Dictionary<string, ParameterExpression> _propertyMap = [];
    private int _paramToUse;

    protected override Expression VisitParameter(ParameterExpression node)
    {
        Guard.Against.Null(node);
        Guard.Against.NullOrWhiteSpace(node.Name);

        if (_propertyMap.TryGetValue(node.Name, out ParameterExpression? value))
        {
            return value;
        }
        else
        {
            ParameterExpression newParam = Expression.Parameter(node.Type, _propertyName[_paramToUse]);
            _propertyMap.Add(node.Name, newParam);
            _paramToUse++;
            return newParam;
        }
    }
}
