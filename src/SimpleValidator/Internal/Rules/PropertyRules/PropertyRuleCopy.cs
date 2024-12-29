using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Internal.Rules.PropertyRules;

internal sealed class PropertyRuleCopy<TEntity, TBundToEntity, TProperty> :
    BasePropertyRule<TEntity, TProperty>
{
    private readonly string _pathToTBoundEntity;
    private readonly Func<TEntity, TBundToEntity> _bundToValueGetter;
    private readonly IValidationRule<TBundToEntity, TProperty> _innerRule;

    public PropertyRuleCopy(
        RuleType type,
        in RuleKey key,
        IValidationRule<TBundToEntity, TProperty> innerRule,
        bool isShortCircuit,
        string pathToTBoundEntity,
        Func<TEntity, TBundToEntity> bundToValueGetter,
        Func<ValidationData<TBundToEntity, TProperty>, string>? errorMsgFactory = null,
        string? errorMsg = null)
        : base(type, key, isShortCircuit)
    {
        _pathToTBoundEntity = pathToTBoundEntity;
        _bundToValueGetter = bundToValueGetter;
        _innerRule = innerRule;
        ErrorMsg = errorMsg;
        ErrorMsgFactory = errorMsgFactory;
    }

    private Func<ValidationData<TBundToEntity, TProperty>, string>? ErrorMsgFactory { get; set; }

    public override bool Failed(ValidationData<TEntity, TProperty> data, [NotNullWhen(true)] out string? errorMsg)
    {
        TBundToEntity oldEntity = _bundToValueGetter(data.EntityValue);

        if (_innerRule.FailsWhen(oldEntity, data.PropertyValue))
        {
            ValidationData<TBundToEntity, TProperty> bundData = new(data.PropertyName, oldEntity, data.PropertyValue);
            errorMsg = ErrorMsg ?? (ErrorMsgFactory == null ?
                _innerRule.GetDefaultMsgTemplate(bundData) :
                ErrorMsgFactory(bundData));
            return true;
        }

        errorMsg = null;
        return false;
    }

    public override void SetErrorMsgFactory(Func<ValidationData<TEntity, TProperty>, string> factory)
    {
        // its not needed.
    }

    public override IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path)
    {
        string newPath = $"{path}.{_pathToTBoundEntity}";
        SelectorKey selectorKey = new SelectorKey(typeof(TNewEntity), typeof(TBundToEntity), _pathToTBoundEntity);
        Func<TNewEntity, TBundToEntity> bundToValueGetter = SelectorsCache.GetOrAdd<TNewEntity, TBundToEntity>(selectorKey, _pathToTBoundEntity);
        RuleKey newKey = Type == RuleType.Comparison ? ComparisonDefinitionUpdater.UpdateDefinition(Key.RuleDefinition, _pathToTBoundEntity) : Key;

        return new PropertyRuleCopy<TNewEntity, TBundToEntity, TProperty>(
            Type,
            newKey,
            _innerRule,
            IsShortCircuit,
            newPath,
            bundToValueGetter,
            ErrorMsgFactory,
            ErrorMsg);
    }
}
