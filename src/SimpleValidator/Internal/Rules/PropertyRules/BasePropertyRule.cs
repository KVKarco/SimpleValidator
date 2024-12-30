using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Internal.Rules.PropertyRules;

internal abstract class BasePropertyRule
{
    public BasePropertyRule(in RuleKey key, bool isShortCircuit)
    {
        Key = key;
        IsShortCircuit = isShortCircuit;
    }

    public RuleKey Key { get; }

    public bool IsShortCircuit { get; }

    protected string? ErrorMsg { get; set; }

    public void SetErrorMsg(string errorMsg) => ErrorMsg = errorMsg;
}

internal abstract class BasePropertyRule<TEntity, TProperty> :
    BasePropertyRule,
    IPropertyRule<TEntity, TProperty>
{
    protected BasePropertyRule(in RuleKey key, bool isShortCircuit)
        : base(key, isShortCircuit)
    {
    }

    public abstract bool Failed(ValidationContext<TEntity, TProperty> context, [NotNullWhen(true)] out string? errorMsg);

    public abstract void SetErrorMsgFactory(Func<IValidationContext<TProperty>, string> errorMsgFactory);

    public abstract void SetErrorMsgFactory(Func<IValidationContext<TEntity, TProperty>, string> errorMsgFactory);

    public abstract IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string missingPath);
}
