namespace SimpleValidator.Rules.Internal;

/// <summary>
/// Represent rule for validating certain property.
/// </summary>
internal interface IValidationRule
{
    string RuleName { get; }
}

/// <summary>
/// Represent rule for validating certain property.
/// </summary>
/// <typeparam name="TEntity">The main type that is validated.</typeparam>
/// <typeparam name="TProperty">Some property that belongs to the main entity.</typeparam>
internal interface IValidationRule<TEntity, TProperty> : IValidationRule
{
    /// <summary>
    /// Method that determines when the rule failed.
    /// </summary>
    /// <param name="entityValue">Main entity value if the rule needs values for comparison.</param>
    /// <param name="propertyValue">Property value that is being validated.</param>
    /// <returns>bool</returns>
    bool FailsWhen(TEntity entityValue, TProperty propertyValue);

    /// <summary>
    /// Method that returns error message when rule failed.
    /// </summary>
    /// <param name="propName">Name of the property.</param>
    /// <param name="entityValue">The value of the main entity if comparison values ​​are required in the error message.</param>
    /// <param name="propertyValue">The value of the validation property if required in the error message.</param>
    /// <returns>string</returns>
    string GetDefaultMsgTemplate(string propName, TEntity entityValue, TProperty propertyValue);
}