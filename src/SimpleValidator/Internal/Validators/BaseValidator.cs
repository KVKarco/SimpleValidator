using SimpleValidator.Exceptions;
using SimpleValidator.Internal.Builders;
using SimpleValidator.Internal.Cache;
using SimpleValidator.Internal.Rules.PropertyRules;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace SimpleValidator.Internal.Validators;

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
}

internal abstract class BaseValidator<TEntity, TPropertyValueFrom, TProperty> :
    BaseValidator,
    IPropertyValidator<TEntity, TPropertyValueFrom>,
    IPropertyValidatorManager<TEntity, TProperty>
{
    private readonly PropertyRulesSet<TEntity, TProperty> _rules;
    private Dictionary<string, IPropertyValidator<TEntity, TProperty>>? _nestedValidators;
    private IPropertyValidator<TEntity, TProperty>? _collectionValidator;

    protected BaseValidator(PropertyOrFieldInfo propertyInfo, NullOptions nullOption, string? propertyPathPrefix = null)
        : base(propertyInfo, nullOption, TypeAvailablePropsCache.GetOrAdd(typeof(TProperty)), propertyPathPrefix)
    {
        _rules = [];
        _nestedValidators = null;
    }

    public PropertyRulesSet<TEntity, TProperty> Rules => _rules;

    public Dictionary<string, IPropertyValidator<TEntity, TProperty>> PropertyValidators => _nestedValidators!;

    public void AddRule(IPropertyRule<TEntity, TProperty> rule)
    {
        if (!Rules.TryAdd(rule))
        {
            throw new DuplicateRuleException(
                $"Validator for property: {PropertyPath} already contains rule with definition: {rule.Key.RuleDefinition}");
        }
    }

    protected void ValidateCore(in ValidationRunContext<TEntity, TPropertyValueFrom> context, TProperty propertyValue)
    {
        if (NullOption == NullOptions.MustBeNull)
        {
            context.AttachMustBeNullError();
            return;
        }

        Collection<string> errorMessages = [];

        ValidationData<TEntity, TProperty> data = new(context.PropertyName, context.EntityValue, propertyValue);

        for (var i = 0; i < Rules.Count; i++)
        {
            if (Rules[i].Failed(data, out string? errorMsg))
            {
                errorMessages.Add(errorMsg);
                if (Rules[i].IsShortCircuit)
                {
                    context.AttachErrors(errorMessages);
                    return;
                }
            }
        }

        context.Result.AddPropertyErrors(context.DisplayName, errorMessages);

        if (_nestedValidators is not null)
        {
            foreach (var propName in AllowedProps.Select(propName => propName.Name))
            {
                if (_nestedValidators.TryGetValue(propName, out var validator))
                {
                    validator.Validate(context.Transform(propertyValue, propName));
                }
            }
        }

        _collectionValidator?.Validate(context.Transform(propertyValue, Info.Name));
    }

    public abstract void Validate(in ValidationRunContext<TEntity, TPropertyValueFrom> context);

    public bool TryGetPropertyValidator(string propertyName, [NotNullWhen(true)] out IPropertyValidator<TEntity, TProperty>? propertyValidator)
    {
        if (_nestedValidators is not null)
        {
            return _nestedValidators.TryGetValue(propertyName, out propertyValidator);
        }

        propertyValidator = null;
        return false;
    }

    public void AddOrReplacePropertyValidator(IPropertyValidator<TEntity, TProperty> propertyValidator)
    {
        _nestedValidators ??= [];
        _nestedValidators[propertyValidator.Info.Name] = propertyValidator;
    }
}
