﻿using SimpleValidator.Internal.Keys;
using SimpleValidator.Rules.Assets;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Rules.PropertyRules;

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

internal abstract class BasePropertyRule<TMainEntity, TProperty> :
    BasePropertyRule,
    IPropertyRule<TMainEntity, TProperty>
{
    protected BasePropertyRule(RuleType type, in RuleKey key, bool isShortCircuit)
        : base(type, key, isShortCircuit)
    {
    }

    public abstract bool Failed(string propName, TMainEntity entityValue, TProperty propertyValue, [NotNullWhen(true)] out string? errorMsg);

    public abstract void SetErrorMsgFactory(Func<string, TMainEntity, TProperty, string> factory);

    public abstract IPropertyRule<TNewEntity, TProperty> Transform<TNewEntity>(string path);
}