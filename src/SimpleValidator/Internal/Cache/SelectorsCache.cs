namespace SimpleValidator.Internal.Cache;

using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.Keys;
using System.Collections.Concurrent;
using System.Linq.Expressions;

internal static class SelectorsCache
{
    private static readonly ConcurrentDictionary<SelectorKey, Delegate> Cache = new ConcurrentDictionary<SelectorKey, Delegate>();

    internal static Func<TEntity, TProperty> GetOrAdd<TEntity, TProperty>(
        in SelectorKey selectorKey,
        Expression<Func<TEntity, TProperty>> selectorExpression)
    {
        if (Cache.TryGetValue(selectorKey, out Delegate? func))
        {
            return (Func<TEntity, TProperty>)func;
        }

        Func<TEntity, TProperty> rewriteFunc = new SelectorExpressionRewriter()
            .VisitAndConvert(selectorExpression, null)
            .Compile();

        Cache.TryAdd(selectorKey, rewriteFunc);

        return rewriteFunc;
    }

    internal static Func<TEntity, TProperty> GetOrAdd<TEntity, TProperty>(
        in SelectorKey selectorKey,
        string path)
    {
        if (Cache.TryGetValue(selectorKey, out Delegate? func))
        {
            return (Func<TEntity, TProperty>)func;
        }

        ParameterExpression paramExpression = Expression.Parameter(typeof(TEntity), "prop");

        Expression body = paramExpression;

        foreach (var member in path.Split('.'))
        {
            body = Expression.PropertyOrField(body, member);
        }

        Func<TEntity, TProperty> rewriteFunc = Expression.Lambda<Func<TEntity, TProperty>>(body, paramExpression).Compile();

        Cache.TryAdd(selectorKey, rewriteFunc);

        return rewriteFunc;
    }
}
