using System.Collections.ObjectModel;

namespace SimpleValidator;

/// <summary>
/// Collection of validation messages from the current validation run.
/// </summary>
public sealed class ValidationResult
{
    private readonly Dictionary<string, List<string>> _errors = [];
    private readonly List<string> _nullWarnings = [];

    /// <summary>
    /// Gets a value indicating whether current validation run produced any errors.
    /// </summary>
    public bool HasErrors => _errors.Count > 0 && _nullWarnings.Count == 0;

    /// <summary>
    /// Gets a value indicating whether validation run produced any reference null warnings.
    /// </summary>
    public bool HasNullWarnings => _nullWarnings.Count > 0;

    /// <summary>
    /// Gets dictionary of error messages for every property if any.
    /// </summary>
    public IReadOnlyDictionary<string, Collection<string>> ValidationErrors
    {
        get
        {
            if (_nullWarnings.Count > 0)
            {
                return new Dictionary<string, Collection<string>>();
            }
            else if (_errors.Count == 0)
            {
                return new Dictionary<string, Collection<string>>();
            }

            return _errors.ToDictionary(x => x.Key, x => new Collection<string>(x.Value));
        }
    }

    /// <summary>
    /// Gets null reference warnings if any.
    /// </summary>
    public IReadOnlyList<string> NullWarnings => [.. this._nullWarnings];

    /// <summary>
    /// Method for adding null reference warnings.
    /// </summary>
    /// <param name="nullWarningMsg">null warning message</param>
    public void AddNullWaring(string nullWarningMsg)
    {
        if (!string.IsNullOrWhiteSpace(nullWarningMsg))
        {
            _nullWarnings.Add(nullWarningMsg);
        }
    }

    /// <summary>
    /// Method for adding property error message.
    /// </summary>
    /// <param name="propertyName">property display name</param>
    /// <param name="errorMessage">error message</param>
    public void AddPropertyError(string propertyName, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrWhiteSpace(errorMessage))
        {
            return;
        }

        if (!_errors.TryGetValue(propertyName, out List<string>? existingErrors))
        {
            _errors.Add(propertyName, [errorMessage]);
        }
        else
        {
            existingErrors.Add(errorMessage);
        }
    }

    /// <summary>
    /// Method for adding property error message or messages.
    /// </summary>
    /// <param name="propertyName">property display name</param>
    /// <param name="errorMessages">error message or messages</param>
    public void AddPropertyErrors(string propertyName, params string[] errorMessages)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || errorMessages is null || errorMessages.Length == 0)
        {
            return;
        }

        List<string> isThereErrors = [];

        for (int i = 0; i < errorMessages.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(errorMessages[i]))
            {
                isThereErrors.Add(errorMessages[i]);
            }
        }

        if (isThereErrors.Count > 0)
        {
            if (!_errors.TryGetValue(propertyName, out List<string>? existingErrors))
            {
                _errors.Add(propertyName, isThereErrors);
            }
            else
            {
                existingErrors.AddRange(isThereErrors);
            }
        }
    }

    /// <summary>
    /// Method for adding property error messages.
    /// </summary>
    /// <param name="propertyName">property display name</param>
    /// <param name="errorMessages">collection of error messages</param>
    public void AddPropertyErrors(string propertyName, Collection<string> errorMessages)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || errorMessages is null || errorMessages.Count == 0)
        {
            return;
        }

        List<string> isThereErrors = [];

        for (int i = 0; i < errorMessages.Count; i++)
        {
            if (!string.IsNullOrWhiteSpace(errorMessages[i]))
            {
                isThereErrors.Add(errorMessages[i]);
            }
        }

        if (isThereErrors.Count > 0)
        {
            if (!_errors.TryGetValue(propertyName, out List<string>? existingErrors))
            {
                _errors.Add(propertyName, isThereErrors);
            }
            else
            {
                existingErrors.AddRange(isThereErrors);
            }
        }
    }
}
