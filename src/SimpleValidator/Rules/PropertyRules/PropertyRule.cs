using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.Keys;
using SimpleValidator.Rules.Assets;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Rules.PropertyRules;

internal sealed class PropertyRule<TMainEntity, TProperty> :
    BasePropertyRule<TMainEntity, TProperty>
{
    private readonly IValidationRule<TMainEntity, TProperty> _innerRule;

    public PropertyRule(RuleType type, in RuleKey key, IValidationRule<TMainEntity, TProperty> innerRule, bool isShortCircuit)
    : base(type, key, isShortCircuit)
    {
        _innerRule = innerRule;
    }

    private Func<string, TMainEntity, TProperty, string>? ErrorMsgFactory { get; set; }

    public override void SetErrorMsgFactory(Func<string, TMainEntity, TProperty, string> errorMsgFactory)
        => ErrorMsgFactory = errorMsgFactory;

    public override bool Failed(string propName, TMainEntity entityValue, TProperty propertyValue, [NotNullWhen(true)] out string? errorMsg)
    {
        if (_innerRule.FailsWhen(entityValue, propertyValue))
        {
            errorMsg = ErrorMsg ?? (ErrorMsgFactory == null ?
                _innerRule.GetDefaultMsgTemplate(propName, entityValue, propertyValue) :
                ErrorMsgFactory(propName, entityValue, propertyValue));
            return true;
        }

        errorMsg = null;
        return false;
    }

    public override IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path)
    {
        SelectorKey selectorKey = new(typeof(TNewEntity), typeof(TMainEntity), path);
        Func<TNewEntity, TMainEntity> bundToValueGetter = SelectorsCache.GetOrAdd<TNewEntity, TMainEntity>(selectorKey, path);
        RuleKey newKey = Type == RuleType.Comparison ? ComparisonDefinitionUpdater.UpdateDefinition(Key.RuleDefinition, path) : Key;

        return new PropertyRuleCopy<TNewEntity, TMainEntity, TProperty>(
            Type,
            newKey,
            _innerRule,
            IsShortCircuit,
            bundToValueGetter,
            ErrorMsgFactory,
            ErrorMsg);
    }
}
