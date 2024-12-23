namespace SimpleValidator.Internal.Cache;

using Ardalis.GuardClauses;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Internal.Keys;
using System.Collections.Concurrent;
using System.Linq.Expressions;

internal static class PredicateCache
{
    private static readonly ConcurrentDictionary<DelegateKey, (Delegate predicate, RuleKey ruleKey)> Cache =
        new ConcurrentDictionary<DelegateKey, (Delegate predicate, RuleKey ruleKey)>();

    internal static Predicate<TProperty> GetOrAdd<TProperty>(
        Expression<Predicate<TProperty>> predicateExpression,
        out RuleKey ruleKey)
    {
        DelegateKey key = DelegateKey.FromExpression(predicateExpression);

        if (Cache.TryGetValue(key, out (Delegate, RuleKey) value))
        {
            ruleKey = value.Item2;
            return (Predicate<TProperty>)value.Item1;
        }

        // Check for validity of the predicate expression:
        Guard.Against.IncorrectPredicate(predicateExpression);

        // Rewrite the predicate.
        Expression<Predicate<TProperty>> correctExpression = new PredicateExpressionRewriter().VisitAndConvert(predicateExpression, null);

        ruleKey = RuleKey.FromPredicate(correctExpression);
        Predicate<TProperty> predicate = correctExpression.Compile();

        // Add it to the cache.
        Cache.TryAdd(key, (predicate, ruleKey));

        return predicate;
    }

    internal static Func<TEntity, TProperty, bool> GetOrAdd<TEntity, TProperty>(
        Expression<Func<TEntity, TProperty, bool>> predicateExpression,
        out RuleKey ruleKey)
    {
        DelegateKey key = DelegateKey.FromExpression(predicateExpression);

        if (Cache.TryGetValue(key, out (Delegate, RuleKey) value))
        {
            ruleKey = value.Item2;
            return (Func<TEntity, TProperty, bool>)value.Item1;
        }

        // Check for validity of the predicate expression:
        Guard.Against.IncorrectPredicate(predicateExpression);

        // Rewrite the predicate.
        Expression<Func<TEntity, TProperty, bool>> correctExpression = new PredicateExpressionRewriter().VisitAndConvert(predicateExpression, null);

        ruleKey = RuleKey.FromPredicate(correctExpression);
        Func<TEntity, TProperty, bool> predicate = correctExpression.Compile();

        // Add it to the cache.
        Cache.TryAdd(key, (predicate, ruleKey));

        return predicate;
    }
}
