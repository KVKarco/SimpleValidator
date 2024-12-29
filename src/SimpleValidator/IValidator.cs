namespace SimpleValidator;

/// <summary>
/// Validator for type.
/// </summary>
/// <typeparam name="TEntity">Type to validate.</typeparam>
public interface IValidator<TEntity>
{
    /// <summary>
    /// Validates TEntity value.
    /// </summary>
    /// <param name="entity">value of the type that is validated.</param>
    /// <returns>ValidationResult</returns>
    ValidationResult Validate(TEntity entity);
}
