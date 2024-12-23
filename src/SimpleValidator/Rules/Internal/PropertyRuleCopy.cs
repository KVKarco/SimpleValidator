namespace SimpleValidator.Rules.Internal;

using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

internal sealed class PropertyRuleCopy<TMainEntity, TBundToEntity, TProperty> :
    PropertyRule,
    IPropertyRule<TMainEntity, TProperty>
{
    private readonly Func<TMainEntity, TBundToEntity> bundToValueGetter;
    private readonly IValidationRule<TBundToEntity, TProperty> innerRule;

    public PropertyRuleCopy(
        RuleType type,
        in RuleKey key,
        IValidationRule<TBundToEntity, TProperty> innerRule,
        bool isShortCircuit,
        Func<TMainEntity, TBundToEntity> bundToValueGetter,
        Func<string, TBundToEntity, TProperty, string>? errorMsgFactory = null,
        string? errorMsg = null)
        : base(type, key, isShortCircuit)
    {
        this.bundToValueGetter = bundToValueGetter;
        this.innerRule = innerRule;
        this.ErrorMsg = errorMsg;
        this.ErrorMsgFactory = errorMsgFactory;
    }

    private Func<string, TBundToEntity, TProperty, string>? ErrorMsgFactory { get; set; }

    public bool Failed(string propName, TMainEntity entityValue, TProperty propertyValue, [NotNullWhen(true)] out string? errorMsg)
    {
        TBundToEntity oldEntity = this.bundToValueGetter(entityValue);

        if (this.innerRule.FailsWhen(oldEntity, propertyValue))
        {
            errorMsg = this.ErrorMsg ?? (this.ErrorMsgFactory == null ?
                this.innerRule.GetDefaultMsgTemplate(propName, oldEntity, propertyValue) :
                this.ErrorMsgFactory(propName, oldEntity, propertyValue));
            return true;
        }

        errorMsg = null;
        return false;
    }

    public void SetErrorMsgFactory(Func<string, TMainEntity, TProperty, string> factory)
    {
        throw new NotImplementedException();
    }

    public IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path)
    {
        SelectorKey selectorKey = new SelectorKey(typeof(TNewEntity), typeof(TBundToEntity), path);
        Func<TNewEntity, TBundToEntity> bundToValueGetter = SelectorsCache.GetOrAdd<TNewEntity, TBundToEntity>(selectorKey, path);
        RuleKey newKey = this.Type == RuleType.Comparison ? ComparisonDefinitionUpdater.UpdateDefinition(this.Key.RuleDefinition, path) : this.Key;

        return new PropertyRuleCopy<TNewEntity, TBundToEntity, TProperty>(
            this.Type,
            newKey,
            this.innerRule,
            this.IsShortCircuit,
            bundToValueGetter,
            this.ErrorMsgFactory,
            this.ErrorMsg);
    }
}
