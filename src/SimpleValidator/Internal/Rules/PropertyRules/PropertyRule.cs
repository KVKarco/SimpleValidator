using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Internal.Rules.PropertyRules;

internal sealed class PropertyRule<TEntity, TProperty> :
    BasePropertyRule<TEntity, TProperty>
{
    private readonly IValidationRule<TEntity, TProperty> _innerRule;

    public PropertyRule(RuleType type, in RuleKey key, IValidationRule<TEntity, TProperty> innerRule, bool isShortCircuit)
    : base(type, key, isShortCircuit)
    {
        _innerRule = innerRule;
    }

    private Func<ValidationData<TEntity, TProperty>, string>? ErrorMsgFactory { get; set; }

    public override void SetErrorMsgFactory(Func<ValidationData<TEntity, TProperty>, string> errorMsgFactory)
        => ErrorMsgFactory = errorMsgFactory;

    public override bool Failed(ValidationData<TEntity, TProperty> data, [NotNullWhen(true)] out string? errorMsg)
    {
        if (_innerRule.FailsWhen(data.EntityValue, data.PropertyValue))
        {
            errorMsg = ErrorMsg ?? (ErrorMsgFactory == null ?
                _innerRule.GetDefaultMsgTemplate(data) :
                ErrorMsgFactory(data));
            return true;
        }

        errorMsg = null;
        return false;
    }

    public override IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path)
    {
        SelectorKey selectorKey = new(typeof(TNewEntity), typeof(TEntity), path);
        Func<TNewEntity, TEntity> bundToValueGetter = SelectorsCache.GetOrAdd<TNewEntity, TEntity>(selectorKey, path);
        RuleKey newKey = Type == RuleType.Comparison ? ComparisonDefinitionUpdater.UpdateDefinition(Key.RuleDefinition, path) : Key;

        return new PropertyRuleCopy<TNewEntity, TEntity, TProperty>(
            Type,
            newKey,
            _innerRule,
            IsShortCircuit,
            path,
            bundToValueGetter,
            ErrorMsgFactory,
            ErrorMsg);
    }
}
