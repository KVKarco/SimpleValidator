using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.Keys;
using SimpleValidator.Internal.Rules.PropertyRules;
using System.Linq.Expressions;

namespace SimpleValidator.Internal.Rules;

internal static class RuleFactory
{
    public static IPropertyRule<TEntity, TProperty> ForPredicate<TEntity, TProperty>(
        Expression<Predicate<TProperty>> predicateExpression, bool isShortCircuit = false)
    {
        Predicate<TProperty> predicate = PredicateCache.GetOrAdd(predicateExpression, out RuleKey key);
        PredicateRule<TProperty> innerRule = new(predicate, key.RuleDefinition);

        return new PropertyRule<TEntity, TProperty>(key, innerRule, isShortCircuit);
    }

    public static IPropertyRule<TEntity, TProperty> ForComparison<TEntity, TProperty>(
        Expression<Func<TEntity, TProperty, bool>> predicateExpression, bool isShortCircuit = false)
    {
        Func<TEntity, TProperty, bool> predicate = PredicateCache.GetOrAdd(predicateExpression, out RuleKey key);
        ComparisonRule<TEntity, TProperty> innerRule = new(predicate, key.RuleDefinition);

        return new PropertyComparisonRule<TEntity, TProperty>(key, innerRule, isShortCircuit);
    }

    public static IPropertyRule<TEntity, TProperty> ForCustom<TEntity, TProperty>(
        IValidationRule<TProperty> customRule, bool isShortCircuit = false)
    {
        return new PropertyRule<TEntity, TProperty>(
            RuleKey.FromString(customRule.RuleName),
            customRule,
            isShortCircuit);
    }

    public static IPropertyRule<TEntity, TProperty> ForCustom<TEntity, TProperty>(
        IValidationRule<TEntity, TProperty> customRule, bool isShortCircuit = false)
    {
        return new PropertyComparisonRule<TEntity, TProperty>(
            RuleKey.FromString(customRule.RuleName),
            customRule,
            isShortCircuit);
    }
}
