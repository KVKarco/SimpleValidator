using Ardalis.GuardClauses;
using SimpleValidator.Internal.GuardsClauses;
using SimpleValidator.Internal.Rules.BuildInRules;

namespace SimpleValidator;

/// <summary>
/// Extension methods for adding custom rules to the current property validator.
/// </summary>
public static class BuildInRulesExtensions
{
    /// <summary>
    /// Adds EqualityRule rule to current property validator.
    /// </summary>
    /// <param name="builder">current property validator builder.</param>
    /// <param name="comparisonValue">Value to be compared with the current property value. </param>
    /// <param name="comparer">equality comparer of TProperty.</param>
    /// <param name="toShortCircuitOnFail">Specifies whether the validation chain for the property stops when the rule fails.</param>
    public static IPropertyRulesBuilder<TEntity, TProperty> EqualTo<TEntity, TProperty>(
        this IPropertyRulesBuilder<TEntity, TProperty> builder,
        TProperty? comparisonValue,
        IEqualityComparer<TProperty>? comparer = null,
        bool toShortCircuitOnFail = false)
    {
        Guard.Against.Null(builder);

        builder.FailsWhen(new EqualityRule<TProperty>(comparisonValue, comparer), toShortCircuitOnFail);
        return builder;
    }

    /// <summary>
    /// Adds EqualityRule rule to current property validator.
    /// </summary>
    /// <param name="builder">current property validator builder.</param>
    /// <param name="func">Func to get value to be compared with the current property value. </param>
    /// <param name="comparer">equality comparer of TProperty.</param>
    /// <param name="toShortCircuitOnFail">Specifies whether the validation chain for the property stops when the rule fails.</param>
    public static IPropertyRulesBuilder<TEntity, TProperty> EqualTo<TEntity, TProperty>(
        this IPropertyRulesBuilder<TEntity, TProperty> builder,
        Func<TEntity, TProperty?> func,
        IEqualityComparer<TProperty>? comparer = null,
        bool toShortCircuitOnFail = false)
    {
        Guard.Against.Null(builder);
        Guard.Against.InternalNull(func);

        builder.FailsWhen(new EqualityRule<TEntity, TProperty>(func, comparer), toShortCircuitOnFail);
        return builder;
    }

    /// <summary>
    /// Adds NotEqualityRule rule to current property validator.
    /// </summary>
    /// <param name="builder">current property validator builder.</param>
    /// <param name="comparisonValue">Value to be compared with the current property value. </param>
    /// <param name="comparer">equality comparer of TProperty.</param>
    /// <param name="toShortCircuitOnFail">Specifies whether the validation chain for the property stops when the rule fails.</param>
    public static IPropertyRulesBuilder<TEntity, TProperty> NotEqualTo<TEntity, TProperty>(
        this IPropertyRulesBuilder<TEntity, TProperty> builder,
        TProperty? comparisonValue,
        IEqualityComparer<TProperty>? comparer = null,
        bool toShortCircuitOnFail = false)
    {
        Guard.Against.Null(builder);

        builder.FailsWhen(new NotEqualityRule<TProperty>(comparisonValue, comparer), toShortCircuitOnFail);
        return builder;
    }

    /// <summary>
    /// Adds NotEqualityRule rule to current property validator.
    /// </summary>
    /// <param name="builder">current property validator builder.</param>
    /// <param name="func">Func to get value to be compared with the current property value. </param>
    /// <param name="comparer">equality comparer of TProperty.</param>
    /// <param name="toShortCircuitOnFail">Specifies whether the validation chain for the property stops when the rule fails.</param>
    public static IPropertyRulesBuilder<TEntity, TProperty> NotEqualTo<TEntity, TProperty>(
        this IPropertyRulesBuilder<TEntity, TProperty> builder,
        Func<TEntity, TProperty?> func,
        IEqualityComparer<TProperty>? comparer = null,
        bool toShortCircuitOnFail = false)
    {
        Guard.Against.Null(builder);
        Guard.Against.InternalNull(func);

        builder.FailsWhen(new NotEqualityRule<TEntity, TProperty>(func, comparer), toShortCircuitOnFail);
        return builder;
    }

    /// <summary>
    /// Adds NotEmptyRule rule to current property validator.
    /// </summary>
    /// <param name="builder">current property validator builder.</param>
    /// <param name="toShortCircuitOnFail">Specifies whether the validation chain for the property stops when the rule fails.</param>
    public static IPropertyRulesBuilder<TEntity, TProperty> NotEmpty<TEntity, TProperty>(
        this IPropertyRulesBuilder<TEntity, TProperty> builder,
        bool toShortCircuitOnFail = false)
    {
        Guard.Against.Null(builder);

        builder.FailsWhen(new NotEmptyRule<TProperty>(), toShortCircuitOnFail);
        return builder;
    }
}
