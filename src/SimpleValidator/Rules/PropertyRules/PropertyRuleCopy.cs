using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.Keys;
using SimpleValidator.Rules.Assets;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Rules.PropertyRules;

internal sealed class PropertyRuleCopy<TMainEntity, TBundToEntity, TProperty> :
    BasePropertyRule<TMainEntity, TProperty>
{
    private readonly Func<TMainEntity, TBundToEntity> _bundToValueGetter;
    private readonly IValidationRule<TBundToEntity, TProperty> _innerRule;

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
        _bundToValueGetter = bundToValueGetter;
        _innerRule = innerRule;
        ErrorMsg = errorMsg;
        ErrorMsgFactory = errorMsgFactory;
    }

    private Func<string, TBundToEntity, TProperty, string>? ErrorMsgFactory { get; set; }

    public override bool Failed(string propName, TMainEntity entityValue, TProperty propertyValue, [NotNullWhen(true)] out string? errorMsg)
    {
        TBundToEntity oldEntity = _bundToValueGetter(entityValue);

        if (_innerRule.FailsWhen(oldEntity, propertyValue))
        {
            errorMsg = ErrorMsg ?? (ErrorMsgFactory == null ?
                _innerRule.GetDefaultMsgTemplate(propName, oldEntity, propertyValue) :
                ErrorMsgFactory(propName, oldEntity, propertyValue));
            return true;
        }

        errorMsg = null;
        return false;
    }

    public override void SetErrorMsgFactory(Func<string, TMainEntity, TProperty, string> factory)
    {
        // its not needed.
    }

    public override IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path)
    {
        SelectorKey selectorKey = new SelectorKey(typeof(TNewEntity), typeof(TBundToEntity), path);
        Func<TNewEntity, TBundToEntity> bundToValueGetter = SelectorsCache.GetOrAdd<TNewEntity, TBundToEntity>(selectorKey, path);
        RuleKey newKey = Type == RuleType.Comparison ? ComparisonDefinitionUpdater.UpdateDefinition(Key.RuleDefinition, path) : Key;

        return new PropertyRuleCopy<TNewEntity, TBundToEntity, TProperty>(
            Type,
            newKey,
            _innerRule,
            IsShortCircuit,
            bundToValueGetter,
            ErrorMsgFactory,
            ErrorMsg);
    }
}
