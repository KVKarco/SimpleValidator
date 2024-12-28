using Ardalis.GuardClauses;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Internal.Keys;
using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace SimpleValidator.Internal.Cache;

/// <summary>
/// Holds compiled predicates from rewritten expressions with correct RuleKeys.
/// </summary>
internal static class PredicateCache
{
    private static readonly ConcurrentDictionary<DelegateKey, (Delegate predicate, RuleKey ruleKey)> _cache = [];

    /// <summary>
    /// Gets or creates delegate and rule key from predicate expression.
    /// </summary>
    internal static Predicate<TProperty> GetOrAdd<TProperty>(
        Expression<Predicate<TProperty>> predicateExpression,
        out RuleKey ruleKey)
    {
        DelegateKey key = DelegateKey.FromExpression(predicateExpression);

        if (_cache.TryGetValue(key, out (Delegate, RuleKey) value))
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
        _cache.TryAdd(key, (predicate, ruleKey));

        return predicate;
    }

    /// <summary>
    /// Gets or creates delegate and rule key from predicate expression.
    /// </summary>
    internal static Func<TEntity, TProperty, bool> GetOrAdd<TEntity, TProperty>(
        Expression<Func<TEntity, TProperty, bool>> predicateExpression,
        out RuleKey ruleKey)
    {
        DelegateKey key = DelegateKey.FromExpression(predicateExpression);

        if (_cache.TryGetValue(key, out (Delegate, RuleKey) value))
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
        _cache.TryAdd(key, (predicate, ruleKey));

        return predicate;
    }
}
