using SimpleValidator.Internal.Keys;
using SimpleValidator.Rules.PropertyRules;
using System.Collections.ObjectModel;

namespace SimpleValidator.Validators.Assets;

internal sealed class PropertyRulesSet<TMainEntity, TProperty> : KeyedCollection<RuleKey, IPropertyRule<TMainEntity, TProperty>>
{
    protected override RuleKey GetKeyForItem(IPropertyRule<TMainEntity, TProperty> item)
    {
        return item.Key;
    }

    public bool TryAdd(IPropertyRule<TMainEntity, TProperty> item)
    {
        if (Contains(item))
        {
            return false;
        }

        Add(item);
        return true;
    }
}
