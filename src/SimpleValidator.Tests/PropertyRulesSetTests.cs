using SimpleValidator.Exceptions;
using SimpleValidator.Internal.Builders;

namespace SimpleValidator.Tests;

public class PropertyRulesSetTests
{
    [Fact]
    public void Should_Have_Four_Rules()
    {
        IValidatorManager<Employee, Employee> validatorManager = new EmployeeValidatorWithFourRulesInFirstNamePropValidator();

        var propertyManager = (IPropertyValidatorManager<Employee, string>)validatorManager.PropertyValidators["FirstName"];

        Assert.Equal(4, propertyManager.Rules.Count);
    }

    [Fact]
    public void Should_Throw_When_Duplicate_PredicateRule_Is_Added()
    {
        Assert.Throws<DuplicateRuleException>(() => new EmployeeValidatorWithDuplicatePredicateRule());
    }

    [Fact]
    public void Should_Throw_When_Nullable_Member_Is_Selected_In_Predicate()
    {
        Assert.Throws<NullableMemberException>(() => new EmployeeValidatorWithNullPredicateMembers());
    }
}

public sealed class EmployeeValidatorWithFourRulesInFirstNamePropValidator : AbstractValidator<Employee>
{
    public EmployeeValidatorWithFourRulesInFirstNamePropValidator()
    {
        ValidationsFor(x => x.FirstName)
            .FailsWhen(x => x.Length < 4 || x.Length > 7).WithErrorMessage("FistName must be more then 4 characters and less the 7")
            .FailsWhen(x => Char.IsLower(x[0])).WithErrorMessage("Fist letter of FirstName must be Uppercase.")
            .FailsWhen(x => string.IsNullOrEmpty(x)).WithErrorMessage("FirstName cant be empty.")
            .FailsWhen(x => x.Contains('@')).WithErrorMessage("FirstName cant contain @");
    }
}

public sealed class EmployeeValidatorWithDuplicatePredicateRule : AbstractValidator<Employee>
{
    public EmployeeValidatorWithDuplicatePredicateRule()
    {
        ValidationsFor(x => x.Age, NullOptions.FailsWhenNull)
            .FailsWhen(x => x < 18).WithErrorMessage("Minimum age for employee is 18.");

        ValidationsFor(x => x.Age, NullOptions.FailsWhenNull)
            .FailsWhen(x => x < 18).WithErrorMessage("Minimum age for employee is 18.");
    }
}

public sealed class EmployeeValidatorWithNullPredicateMembers : AbstractValidator<Employee>
{
    public EmployeeValidatorWithNullPredicateMembers()
    {
        ValidationsFor(x => x.PersonalInfo).FailsWhen(x => x.Address.City == "a");
    }
}
