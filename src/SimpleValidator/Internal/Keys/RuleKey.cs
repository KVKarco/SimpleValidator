namespace SimpleValidator.Internal.Keys;

using System.Linq.Expressions;

internal readonly record struct RuleKey
{
    private RuleKey(string ruleName)
    {
        this.RuleDefinition = ruleName;
    }

    internal readonly string RuleDefinition { get; }

    public static RuleKey FromPredicate<TProperty>(Expression<Predicate<TProperty>> expression)
    {
        return new RuleKey(expression.ToString());
    }

    public static RuleKey FromPredicate<TEntity, TProperty>(Expression<Func<TEntity, TProperty, bool>> expression)
    {
        return new RuleKey(expression.ToString());
    }

    public static RuleKey FromString(string definition)
    {
        return new RuleKey(definition);
    }
}
