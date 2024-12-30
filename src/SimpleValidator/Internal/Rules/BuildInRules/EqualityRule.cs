namespace SimpleValidator.Internal.Rules.BuildInRules;

internal sealed class EqualityRule<TProperty> : AbstractRule<TProperty>
{
    private readonly TProperty? _comparisonValue;
    private readonly IEqualityComparer<TProperty>? _comparer;

    internal EqualityRule(TProperty? value, IEqualityComparer<TProperty>? comparer = null) :
        base(nameof(EqualityRule<TProperty>))
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
            return !_comparer.Equals(propertyValue, _comparisonValue);
        }

        return !propertyValue!.Equals(_comparisonValue);
    }

    public override string GetDefaultMsgTemplate(IValidationContext<TProperty> context)
    {
        return DefaultErrorMessages.MustBeEqual(context.PropertyName, _comparisonValue);
    }
}

internal sealed class EqualityRule<TEntity, TProperty> : AbstractRule<TEntity, TProperty>
{
    private readonly Func<TEntity, TProperty?> _comparisonValueGetter;
    private readonly IEqualityComparer<TProperty>? _comparer;

    internal EqualityRule(Func<TEntity, TProperty?> comparisonValueGetter, IEqualityComparer<TProperty>? comparer = null) :
        base(nameof(EqualityRule<TEntity, TProperty>))
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
            return !_comparer.Equals(propertyValue, value);
        }

        return !propertyValue!.Equals(value);
    }

    public override string GetDefaultMsgTemplate(IValidationContext<TEntity, TProperty> context)
    {
        return DefaultErrorMessages.MustBeEqual(context.PropertyName, _comparisonValueGetter(context.EntityValue));
    }
}