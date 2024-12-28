using Ardalis.GuardClauses;
using SimpleValidator.Internal.GuardsClauses;

namespace SimpleValidator.Rules;

/// <summary>
/// Abstract class for creating custom rules.
/// </summary>
public abstract class AbstractRule<TEntity, TProperty> : IValidationRule<TEntity, TProperty>
{
    /// <summary>
    /// Creates custom rule whit unique rule name.
    /// </summary>
    /// <param name="ruleName">unique rule name.</param>
    protected AbstractRule(string ruleName)
    {
        RuleName = Guard.Against.InternalNullOrWhiteSpace(ruleName);
    }

    /// <summary>
    /// Unique name for the rule.
    /// </summary>
    public string RuleName { get; }

    bool IValidationRule<TEntity, TProperty>.FailsWhen(TEntity entityValue, TProperty propertyValue)
        => FailsWhen(entityValue, propertyValue);

    string IValidationRule<TEntity, TProperty>.GetDefaultMsgTemplate(string propName, TEntity entityValue, TProperty propertyValue)
        => GetDefaultMsgTemplate(propName, entityValue, propertyValue);

    /// <summary>
    /// Method that determines when the rule failed.
    /// </summary>
    /// <param name="entityValue">Main entity value if the rule needs values for comparison.</param>
    /// <param name="propertyValue">Property value that is being validated.</param>
    /// <returns>bool</returns>
    public abstract bool FailsWhen(TEntity entityValue, TProperty propertyValue);

    /// <summary>
    /// Method that returns error message when rule failed.
    /// </summary>
    /// <param name="propName">Name of the property.</param>
    /// <param name="entityValue">The value of the main entity if comparison values ​​are required in the error message.</param>
    /// <param name="propertyValue">The value of the validation property if required in the error message.</param>
    /// <returns>string</returns>
    public abstract string GetDefaultMsgTemplate(string propName, TEntity entityValue, TProperty propertyValue);
}
