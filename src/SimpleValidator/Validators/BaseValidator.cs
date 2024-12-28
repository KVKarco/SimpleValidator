using SimpleValidator.Builders.Internal;
using SimpleValidator.Internal;
using SimpleValidator.Internal.Cache;
using SimpleValidator.Rules.PropertyRules;
using SimpleValidator.Validators.Assets;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace SimpleValidator.Validators;

internal class BaseValidator : IValidatorInfo
{
    protected BaseValidator(
        PropertyOrFieldInfo propertyInfo,
        NullOptions nullOption,
        AvailablePropsForValidating nestedProps,
        string? propertyPathPrefix = null)
    {
        Info = propertyInfo;
        NullOption = nullOption;
        PropertyPath = propertyPathPrefix == null ? Info.Name : propertyPathPrefix + '.' + Info.Name;
        AllowedProps = nestedProps;
    }

    public AvailablePropsForValidating AllowedProps { get; }

    public PropertyOrFieldInfo Info { get; }

    public NullOptions NullOption { get; }

    public string PropertyPath { get; }

    protected string ResolveDisplayName(int? elementIndex)
    {
        return elementIndex == null ?
            PropertyPath :
            PropertyPath.Replace("elementIndex", elementIndex.Value.ToString(CultureInfo.InvariantCulture), StringComparison.OrdinalIgnoreCase);
    }

    protected string ResolveName(int? elementIndex)
    {
        return elementIndex == null ?
            Info.Name :
            $"{Info.Name}[{elementIndex}]";
    }
}

internal abstract class BaseValidator<TMainEntity, TPropertyValueFrom, TProperty> :
    BaseValidator,
    IPropertyValidator<TMainEntity, TPropertyValueFrom>,
    IPropertyValidatorManager<TMainEntity, TProperty>
{
    private readonly PropertyRulesSet<TMainEntity, TProperty> _rules;
    private Dictionary<string, IPropertyValidator<TMainEntity, TProperty>>? _nestedValidators;
    private IPropertyValidator<TMainEntity, TProperty>? _collectionValidator;

    protected BaseValidator(PropertyOrFieldInfo propertyInfo, NullOptions nullOption, string? propertyPathPrefix = null)
        : base(propertyInfo, nullOption, TypeAvailablePropsCache.GetOrAdd(typeof(TProperty)), propertyPathPrefix)
    {
        _rules = [];
        _nestedValidators = null;
    }

    public PropertyRulesSet<TMainEntity, TProperty> Rules => _rules;

    public Dictionary<string, IPropertyValidator<TMainEntity, TProperty>> PropertyValidators => _nestedValidators!;

    public void AddRule(IPropertyRule<TMainEntity, TProperty> rule)
    {
        if (!Rules.TryAdd(rule))
        {
            throw new ValidatorArgumentException(
                $"Validator for property: {PropertyPath} already contains rule with definition: {rule.Key.RuleDefinition}");
        }
    }

    protected void ValidateCore(
        TMainEntity entityValue,
        TProperty propertyValue,
        in string name,
        in string displayName,
        [NotNull] ValidationResult result,
        int? elementIndex = null)
    {
        if (NullOption == NullOptions.MustBeNull)
        {
            result.AddPropertyError(displayName, $"{name} {DefaultErrorMessages._mustBeNull}");
            return;
        }

        Collection<string> errorMessages = [];

        for (var i = 0; i < Rules.Count; i++)
        {
            if (Rules[i].Failed(name ?? Info.Name, entityValue, propertyValue, out string? errorMsg))
            {
                errorMessages.Add(errorMsg);
                if (Rules[i].IsShortCircuit)
                {
                    result.AddPropertyErrors(displayName, errorMessages);
                    return;
                }
            }
        }

        result.AddPropertyErrors(displayName, errorMessages);

        if (_nestedValidators is not null)
        {
            foreach (var propName in AllowedProps.Select(propName => propName.Name))
            {
                if (_nestedValidators.TryGetValue(propName, out var validator))
                {
                    validator.Validate(entityValue, propertyValue, result, elementIndex);
                }
            }
        }

        _collectionValidator?.Validate(entityValue, propertyValue, result, elementIndex);
    }

    public abstract void Validate(
        TMainEntity entityValue,
        TPropertyValueFrom propertyValueFrom,
        [NotNull] ValidationResult result,
        int? elementIndex = null);

    public bool TryGetPropertyValidator(string propertyName, [NotNullWhen(true)] out IPropertyValidator<TMainEntity, TProperty>? propertyValidator)
    {
        if (_nestedValidators is not null)
        {
            return _nestedValidators.TryGetValue(propertyName, out propertyValidator);
        }

        propertyValidator = null;
        return false;
    }

    public void AddOrReplacePropertyValidator(IPropertyValidator<TMainEntity, TProperty> propertyValidator)
    {
        _nestedValidators ??= [];
        _nestedValidators[propertyValidator.Info.Name] = propertyValidator;
    }
}
