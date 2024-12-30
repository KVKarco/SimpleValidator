using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.ExpressionHelpers;
using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Internal.Rules.PropertyRules;

internal sealed class PropertyComparisonRule<TEntity, TProperty> :
    BasePropertyRule<TEntity, TProperty>
{
    private readonly IValidationRule<TEntity, TProperty> _innerRule;

    public PropertyComparisonRule(in RuleKey key, IValidationRule<TEntity, TProperty> innerRule, bool isShortCircuit)
    : base(key, isShortCircuit)
    {
        _innerRule = innerRule;
    }

    public PropertyComparisonRule(
        in RuleKey key,
        IValidationRule<TEntity, TProperty> innerRule,
        bool isShortCircuit,
        string? errMsg = null,
        Func<IValidationContext<TEntity, TProperty>, string>? factory = null)
    : base(key, isShortCircuit)
    {
        _innerRule = innerRule;
        ErrorMsg = errMsg;
        ErrorMsgFactory = factory;
    }

    private Func<IValidationContext<TEntity, TProperty>, string>? ErrorMsgFactory { get; set; }

    public override void SetErrorMsgFactory(Func<IValidationContext<TProperty>, string> errorMsgFactory)
        => throw new NotImplementedException();

    public override void SetErrorMsgFactory(Func<IValidationContext<TEntity, TProperty>, string> errorMsgFactory)
        => ErrorMsgFactory = errorMsgFactory;

    public override bool Failed(ValidationContext<TEntity, TProperty> context, [NotNullWhen(true)] out string? errorMsg)
    {
        if (_innerRule.FailsWhen(context.EntityValue, context.PropertyValue))
        {
            errorMsg = ErrorMsg ?? (ErrorMsgFactory == null ?
                _innerRule.GetDefaultMsgTemplate(context) :
                ErrorMsgFactory(context));
            return true;
        }

        errorMsg = null;
        return false;
    }

    public override IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string missingPath)
    {
        SelectorKey selectorKey = new(typeof(TNewEntity), typeof(TEntity), missingPath);
        Func<TNewEntity, TEntity> bundToValueGetter = SelectorsCache.GetOrAdd<TNewEntity, TEntity>(selectorKey, missingPath);
        RuleKey newKey = ComparisonDefinitionUpdater.UpdateDefinition(Key.RuleDefinition, missingPath);

        return new PropertyRuleCopy<TNewEntity, TEntity, TProperty>(
            newKey,
            _innerRule,
            IsShortCircuit,
            missingPath,
            bundToValueGetter,
            ErrorMsgFactory,
            ErrorMsg);
    }
}
