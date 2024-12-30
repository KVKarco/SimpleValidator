namespace SimpleValidator.Internal.Rules.BuildInRules;

internal sealed class NotEqualityRule<TProperty> : AbstractRule<TProperty>
{
    private readonly TProperty? _comparisonValue;
    private readonly IEqualityComparer<TProperty>? _comparer;

    internal NotEqualityRule(TProperty? value, IEqualityComparer<TProperty>? comparer = null) :
        base(nameof(NotEqualityRule<TProperty>))
    {
        _comparisonValue = value;
        _comparer = comparer;
    }

    public override bool FailsWhen(TProperty propertyValue)
    {
        if (_comparisonValue is null)
        {
            return true;
        }

        if (_comparer != null)
        {
            return _comparer.Equals(propertyValue, _comparisonValue);
        }

        return Equals(propertyValue, _comparisonValue);
    }

    public override string GetDefaultMsgTemplate(IValidationContext<TProperty> context)
    {
        return DefaultErrorMessages.CantBeEqual(context.PropertyName, _comparisonValue);
    }
}

internal sealed class NotEqualityRule<TEntity, TProperty> : AbstractRule<TEntity, TProperty>
{
    private readonly Func<TEntity, TProperty?> _comparisonValueGetter;
    private readonly IEqualityComparer<TProperty>? _comparer;

    internal NotEqualityRule(Func<TEntity, TProperty?> comparisonValueGetter, IEqualityComparer<TProperty>? comparer = null) :
        base(nameof(NotEqualityRule<TEntity, TProperty>))
    {
        _comparer = comparer;
        _comparisonValueGetter = comparisonValueGetter;
    }

    public override bool FailsWhen(TEntity entityValue, TProperty propertyValue)
    {
        TProperty? value = _comparisonValueGetter(entityValue);

        if (value is null)
        {
            return true;
        }

        if (_comparer != null)
        {
            return _comparer.Equals(propertyValue, value);
        }

        return Equals(propertyValue, value);
    }

    public override string GetDefaultMsgTemplate(IValidationContext<TEntity, TProperty> context)
    {
        return DefaultErrorMessages.CantBeEqual(context.PropertyName, _comparisonValueGetter(context.EntityValue));
    }
}