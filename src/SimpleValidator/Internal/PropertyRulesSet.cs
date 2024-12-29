using SimpleValidator.Internal.Keys;
using SimpleValidator.Internal.Rules.PropertyRules;
using System.Collections.ObjectModel;

namespace SimpleValidator.Internal;

internal sealed class PropertyRulesSet<TEntity, TProperty> : KeyedCollection<RuleKey, IPropertyRule<TEntity, TProperty>>
{
    protected override RuleKey GetKeyForItem(IPropertyRule<TEntity, TProperty> item)
    {
        return item.Key;
    }

    public bool TryAdd(IPropertyRule<TEntity, TProperty> item)
    {
        if (Contains(item.Key))
        {
            return false;
        }

        Add(item);
        return true;
    }
}
