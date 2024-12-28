using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.Keys;
using SimpleValidator.Rules.PropertyRules;
using System.Linq.Expressions;

namespace SimpleValidator.Rules.Assets;

internal static class RuleFactory
{
    public static IPropertyRule<TMainEntity, TProperty> ForPredicate<TMainEntity, TProperty>(
        Expression<Predicate<TProperty>> predicateExpression, bool isShortCircuit = false)
    {
        Predicate<TProperty> predicate = PredicateCache.GetOrAdd(predicateExpression, out RuleKey key);
        PredicateRule<TMainEntity, TProperty> innerRule = new(predicate, key.RuleDefinition);

        return new PropertyRule<TMainEntity, TProperty>(RuleType.Predicate, key, innerRule, isShortCircuit);
    }

    public static IPropertyRule<TMainEntity, TProperty> ForComparison<TMainEntity, TProperty>(
        Expression<Func<TMainEntity, TProperty, bool>> predicateExpression, bool isShortCircuit = false)
    {
        Func<TMainEntity, TProperty, bool> predicate = PredicateCache.GetOrAdd(predicateExpression, out RuleKey key);
        ComparisonRule<TMainEntity, TProperty> innerRule = new(predicate, key.RuleDefinition);

        return new PropertyRule<TMainEntity, TProperty>(RuleType.Comparison, key, innerRule, isShortCircuit);
    }

    public static IPropertyRule<TMainEntity, TProperty> ForCustom<TMainEntity, TProperty>(
        AbstractRule<TMainEntity, TProperty> customRule, bool isShortCircuit = false)
    {
        return new PropertyRule<TMainEntity, TProperty>(
            RuleType.Custom,
            RuleKey.FromString(customRule.RuleName),
            customRule,
            isShortCircuit);
    }
}
