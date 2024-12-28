using Ardalis.GuardClauses;
using SimpleValidator.Internal.GuardsClauses;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace SimpleValidator.Internal.Cache;

internal static class TypeAvailablePropsCache
{
    private static readonly ConcurrentDictionary<Type, AvailablePropsForValidating> _cache = [];
    private static readonly NullabilityInfoContext _nullabilityInfoContext = new();

    private static bool IsNullable([NotNull] PropertyInfo type)
    {
        NullabilityState state = _nullabilityInfoContext.Create(type).ReadState;

        return state == NullabilityState.Nullable;
    }

    private static bool IsNullable([NotNull] FieldInfo type)
    {
        NullabilityState state = _nullabilityInfoContext.Create(type).ReadState;

        return state == NullabilityState.Nullable;
    }

    private static AvailablePropsForValidating CreateAvailablePropsForType([NotNull] Type type)
    {
        Type notNullableType = Nullable.GetUnderlyingType(type) ?? type;

        AvailablePropsForValidating availableProps = [];

        //get all public properties of type.
        Dictionary<string, PropertyInfo> properties = notNullableType.GetProperties(
            BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(static x => x.Name, static x => x);

#pragma warning disable S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

        // add all internal, protected internal properties.
        notNullableType.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
            .Where(p =>
            p.GetGetMethod(true)?.IsAssembly == true ||
            p.GetGetMethod(true)?.IsFamilyOrAssembly == true)
            .ToList()
            .ForEach(p => properties.TryAdd(p.Name, p));

        // get all fields of type so context can be created in order of declaration.
        FieldInfo[] fields = notNullableType.GetFields(
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
#pragma warning restore S3011 // Reflection should not be used to increase accessibility of classes, methods, or fields

        foreach (FieldInfo field in fields)
        {
            if (field.Name.Contains("BackingField", StringComparison.OrdinalIgnoreCase))
            {
                var backingFieldName = field.Name;
                int startIndex = backingFieldName.IndexOf('<', StringComparison.Ordinal);
                int endIndex = backingFieldName.IndexOf('>', StringComparison.Ordinal);

                string correctName = backingFieldName
                    .Substring(startIndex + 1, endIndex - startIndex - 1);

                if (properties.TryGetValue(correctName, out PropertyInfo? propertyInfo)
                    && propertyInfo.PropertyType.AssemblyQualifiedName != null)
                {
                    PropertyOrFieldInfo item = new(
                        propertyInfo.Name,
                        propertyInfo.PropertyType,
                        type,
                        IsNullable(propertyInfo));

                    availableProps.TryAdd(item);
                }
            }
            else
            {
                if ((!field.IsPrivate || !field.IsFamily || !field.IsFamilyAndAssembly)
                    && field.FieldType.AssemblyQualifiedName != null)
                {
                    PropertyOrFieldInfo item = new(
                        field.Name,
                        field.FieldType,
                        type,
                        IsNullable(field));

                    availableProps.TryAdd(item);
                }
            }
        }

        return availableProps;
    }

    internal static AvailablePropsForValidating GetOrAdd([NotNull] Type type)
    {
        Guard.Against.UnsupportedType(type);

        if (_cache.TryGetValue(type, out AvailablePropsForValidating? availableProps))
        {
            return availableProps;
        }

        AvailablePropsForValidating allowedProps = CreateAvailablePropsForType(type);

        _cache.TryAdd(type, allowedProps);

        return allowedProps;
    }
}
