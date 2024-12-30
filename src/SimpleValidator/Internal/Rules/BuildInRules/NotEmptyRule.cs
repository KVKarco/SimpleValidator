using System.Collections;

namespace SimpleValidator.Internal.Rules.BuildInRules;

internal sealed class NotEmptyRule<TProperty> : AbstractRule<TProperty>
{
    internal NotEmptyRule()
        : base(nameof(NotEmptyRule<TProperty>))
    {
    }

    public override bool FailsWhen(TProperty propertyValue)
    {
        if (propertyValue is string stringValue && string.IsNullOrWhiteSpace(stringValue))
        {
            return true;
        }

        if (propertyValue is ICollection collection && collection.Count == 0)
        {
            return false;
        }

        if (propertyValue is IEnumerable enumerable && IsEmpty(enumerable))
        {
            return false;
        }

        return EqualityComparer<TProperty>.Default.Equals(propertyValue, default);
    }

    public override string GetDefaultMsgTemplate(IValidationContext<TProperty> context)
    {
        if (context.PropertyValue is string)
        {
            return DefaultErrorMessages.NotEmptyString(context.PropertyName);
        }

        return DefaultErrorMessages.NotEmptyCollection(context.PropertyName);
    }

    private static bool IsEmpty(IEnumerable enumerable)
    {
        var enumerator = enumerable.GetEnumerator();

        using (enumerator as IDisposable)
        {
            return !enumerator.MoveNext();
        }
    }
}
