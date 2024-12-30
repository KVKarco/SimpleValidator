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
        in RuleKey key,
        IValidationRule<TBundToEntity, TProperty> innerRule,
        bool isShortCircuit,
        string pathToTBoundEntity,
        Func<TEntity, TBundToEntity> bundToValueGetter,
        Func<IValidationContext<TBundToEntity, TProperty>, string>? errorMsgFactory = null,
        string? errorMsg = null)
        : base(key, isShortCircuit)
    {
        _pathToTBoundEntity = pathToTBoundEntity;
        _bundToValueGetter = bundToValueGetter;
        _innerRule = innerRule;
        ErrorMsg = errorMsg;
        ErrorMsgFactory = errorMsgFactory;
    }

    private Func<IValidationContext<TBundToEntity, TProperty>, string>? ErrorMsgFactory { get; set; }

    public override bool Failed(ValidationContext<TEntity, TProperty> context, [NotNullWhen(true)] out string? errorMsg)
    {
        TBundToEntity oldEntity = _bundToValueGetter(context.EntityValue);

        if (_innerRule.FailsWhen(oldEntity, context.PropertyValue))
        {
            var oldContext = context.Transform(oldEntity);

            errorMsg = ErrorMsg ?? (ErrorMsgFactory == null ?
                _innerRule.GetDefaultMsgTemplate(oldContext) :
                ErrorMsgFactory(oldContext));
            return true;
        }

        errorMsg = null;
        return false;
    }

    public override void SetErrorMsgFactory(Func<IValidationContext<TEntity, TProperty>, string> factory)
        => throw new NotImplementedException();

    public override void SetErrorMsgFactory(Func<IValidationContext<TProperty>, string> factory)
        => throw new NotImplementedException();

    public override IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string missingPath)
    {
        string newPath = $"{missingPath}.{_pathToTBoundEntity}";
        SelectorKey selectorKey = new SelectorKey(typeof(TNewEntity), typeof(TBundToEntity), newPath);
        Func<TNewEntity, TBundToEntity> bundToValueGetter = SelectorsCache.GetOrAdd<TNewEntity, TBundToEntity>(selectorKey, newPath);
        RuleKey newKey = ComparisonDefinitionUpdater.UpdateDefinition(Key.RuleDefinition, missingPath);

        return new PropertyRuleCopy<TNewEntity, TBundToEntity, TProperty>(
            newKey,
            _innerRule,
            IsShortCircuit,
            newPath,
            bundToValueGetter,
            ErrorMsgFactory,
            ErrorMsg);
    }
}
