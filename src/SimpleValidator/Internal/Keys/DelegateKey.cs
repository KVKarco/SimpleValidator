using System.Linq.Expressions;

namespace SimpleValidator.Internal.Keys;

/// <summary>
/// Used to get correct delegate from cache.
/// </summary>
internal readonly record struct DelegateKey
{
    private DelegateKey(Type propertyType, string definition, Type? fromProperty = null)
    {
        ForProperty = propertyType;
        MethodDefinition = definition;
        FromProperty = fromProperty;
    }

    public readonly Type? FromProperty { get; }

    public readonly Type ForProperty { get; }

    public readonly string MethodDefinition { get; }

    public static DelegateKey FromExpression<TProperty>(Expression<Predicate<TProperty>> expression)
    {
        return new DelegateKey(typeof(TProperty), expression.ToString());
    }

    public static DelegateKey FromExpression<TEntity, TProperty>(Expression<Func<TEntity, TProperty, bool>> expression)
    {
        return new DelegateKey(typeof(TProperty), expression.ToString(), typeof(TEntity));
    }
}
