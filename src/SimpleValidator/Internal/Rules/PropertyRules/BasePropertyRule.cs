using SimpleValidator.Internal.Keys;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Internal.Rules.PropertyRules;

internal abstract class BasePropertyRule
{
    public BasePropertyRule(RuleType type, in RuleKey key, bool isShortCircuit)
    {
        Type = type;
        Key = key;
        IsShortCircuit = isShortCircuit;
    }

    public RuleKey Key { get; }

    public bool IsShortCircuit { get; }

    protected RuleType Type { get; }

    protected string? ErrorMsg { get; set; }

    public void SetErrorMsg(string errorMsg) => ErrorMsg = errorMsg;
}

internal abstract class BasePropertyRule<TEntity, TProperty> :
    BasePropertyRule,
    IPropertyRule<TEntity, TProperty>
{
    protected BasePropertyRule(RuleType type, in RuleKey key, bool isShortCircuit)
        : base(type, key, isShortCircuit)
    {
    }

    public abstract bool Failed(ValidationData<TEntity, TProperty> data, [NotNullWhen(true)] out string? errorMsg);

    public abstract void SetErrorMsgFactory(Func<ValidationData<TEntity, TProperty>, string> errorMsgFactory);

    public abstract IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path);
}
