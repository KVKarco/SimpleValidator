using System.Collections.ObjectModel;

namespace SimpleValidator.Internal;

internal sealed class AvailablePropsForValidating : KeyedCollection<string, PropertyOrFieldInfo>
{
    protected override string GetKeyForItem(PropertyOrFieldInfo item)
    {
        return item.Name;
    }

    internal bool TryAdd(PropertyOrFieldInfo item)
    {
        if (Contains(item.Name))
        {
            return false;
        }

        Add(item);
        return true;
    }

    internal PropertyOrFieldInfo GetInfoOrThrow(string propertyName)
    {
        if (TryGetValue(propertyName, out var property))
        {
            return property;
        }

        throw new ValidatorArgumentException($"Property with name: {propertyName} its not available for validation.");
    }
}
