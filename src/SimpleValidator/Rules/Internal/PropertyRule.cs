namespace SimpleValidator.Rules.Internal;

using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

internal class PropertyRule
{
    public PropertyRule(RuleType type, in RuleKey key, bool isShortCircuit)
    {
        this.Type = type;
        this.Key = key;
        this.IsShortCircuit = isShortCircuit;
    }

    public RuleKey Key { get; }

    public bool IsShortCircuit { get; }

    protected RuleType Type { get; }

    protected string? ErrorMsg { get; set; }

    public static IPropertyRule<TMainEntity, TProperty> ForPredicateRule<TMainEntity, TProperty>(
        Expression<Predicate<TProperty>> predicateExpression, bool isShortCircuit = false)
    {
        Predicate<TProperty> predicate = PredicateCache.GetOrAdd(predicateExpression, out RuleKey key);
        PredicateRule<TMainEntity, TProperty> innerRule = new PredicateRule<TMainEntity, TProperty>(predicate, key.RuleDefinition);

        return new PropertyRule<TMainEntity, TProperty>(RuleType.Predicate, key, innerRule, isShortCircuit);
    }

    public static IPropertyRule<TMainEntity, TProperty> ForComparisonRule<TMainEntity, TProperty>(
        Expression<Func<TMainEntity, TProperty, bool>> predicateExpression, bool isShortCircuit = false)
    {
        Func<TMainEntity, TProperty, bool> predicate = PredicateCache.GetOrAdd(predicateExpression, out RuleKey key);
        ComparisonRule<TMainEntity, TProperty> innerRule = new ComparisonRule<TMainEntity, TProperty>(predicate, key.RuleDefinition);

        return new PropertyRule<TMainEntity, TProperty>(RuleType.Comparison, key, innerRule, isShortCircuit);
    }

    public static IPropertyRule<TMainEntity, TProperty> ForCustomRule<TMainEntity, TProperty>(
        AbstractRule<TMainEntity, TProperty> customRule, bool isShortCircuit = false)
    {
        return new PropertyRule<TMainEntity, TProperty>(
            RuleType.Custom,
            RuleKey.FromString(customRule.RuleName),
            customRule,
            isShortCircuit);
    }

    public void SetErrorMsg(string errorMsg) => this.ErrorMsg = errorMsg;
}

internal class PropertyRule<TMainEntity, TProperty> :
    PropertyRule,
    IPropertyRule<TMainEntity, TProperty>
{
    private readonly IValidationRule<TMainEntity, TProperty> innerRule;

    public PropertyRule(RuleType type, in RuleKey key, IValidationRule<TMainEntity, TProperty> innerRule, bool isShortCircuit)
    : base(type, key, isShortCircuit)
    {
        this.innerRule = innerRule;
    }

    private Func<string, TMainEntity, TProperty, string>? ErrorMsgFactory { get; set; }

    public void SetErrorMsgFactory(Func<string, TMainEntity, TProperty, string> errorMsgFactory)
        => this.ErrorMsgFactory = errorMsgFactory;

    public bool Failed(string propName, TMainEntity entityValue, TProperty propertyValue, [NotNullWhen(true)] out string? errorMsg)
    {
        if (this.innerRule.FailsWhen(entityValue, propertyValue))
        {
            errorMsg = this.ErrorMsg ?? (this.ErrorMsgFactory == null ?
                this.innerRule.GetDefaultMsgTemplate(propName, entityValue, propertyValue) :
                this.ErrorMsgFactory(propName, entityValue, propertyValue));
            return true;
        }

        errorMsg = null;
        return false;
    }

    public IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path)
    {
        SelectorKey selectorKey = new SelectorKey(typeof(TNewEntity), typeof(TMainEntity), path);
        Func<TNewEntity, TMainEntity> bundToValueGetter = SelectorsCache.GetOrAdd<TNewEntity, TMainEntity>(selectorKey, path);
        RuleKey newKey = this.Type == RuleType.Comparison ? ComparisonDefinitionUpdater.UpdateDefinition(this.Key.RuleDefinition, path) : this.Key;

        return new PropertyRuleCopy<TNewEntity, TMainEntity, TProperty>(
            this.Type,
            newKey,
            this.innerRule,
            this.IsShortCircuit,
            bundToValueGetter,
            this.ErrorMsgFactory,
            this.ErrorMsg);
    }
}
