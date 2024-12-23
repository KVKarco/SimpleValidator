namespace SimpleValidator.Internal.Keys;

using System.Linq.Expressions;

internal readonly record struct DelegateKey
{
    private DelegateKey(Type propertyType, string definition, Type? fromProperty = null)
    {
        this.ForProperty = propertyType;
        this.MethodDefinition = definition;
        this.FromProperty = fromProperty;
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

    public static DelegateKey FromExpression<TProperty>(Expression<Func<string, TProperty, string>> expression)
    {
        return new DelegateKey(typeof(TProperty), expression.ToString());
    }
}
