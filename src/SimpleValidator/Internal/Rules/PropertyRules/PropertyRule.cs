using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Internal.Rules.PropertyRules;

internal sealed class PropertyRule<TEntity, TProperty> :
    BasePropertyRule<TEntity, TProperty>
{
    private readonly IValidationRule<TProperty> _innerRule;

    public PropertyRule(in RuleKey key, IValidationRule<TProperty> innerRule, bool isShortCircuit)
    : base(key, isShortCircuit)
    {
        _innerRule = innerRule;
    }

    public PropertyRule(
        in RuleKey key,
        IValidationRule<TProperty> innerRule,
        bool isShortCircuit,
        string? errMsg = null,
        Func<IValidationContext<TProperty>, string>? factory = null)
    : base(key, isShortCircuit)
    {
        _innerRule = innerRule;
        ErrorMsg = errMsg;
        ErrorMsgFactory = factory;
    }

    private Func<IValidationContext<TProperty>, string>? ErrorMsgFactory { get; set; }

    public override void SetErrorMsgFactory(Func<IValidationContext<TProperty>, string> errorMsgFactory)
        => ErrorMsgFactory = errorMsgFactory;

    public override void SetErrorMsgFactory(Func<IValidationContext<TEntity, TProperty>, string> errorMsgFactory)
        => throw new NotImplementedException();

    public override bool Failed(ValidationContext<TEntity, TProperty> context, [NotNullWhen(true)] out string? errorMsg)
    {
        if (_innerRule.FailsWhen(context.PropertyValue))
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
        return new PropertyRule<TNewEntity, TProperty>(Key, _innerRule, IsShortCircuit, ErrorMsg, ErrorMsgFactory);
    }
}
