using SimpleValidator.Exceptions;
using SimpleValidator.Internal.Builders;

namespace SimpleValidator.Tests;

public class PropertyRulesSetTests
{
    [Fact]
    public void Should_Have_Five_Build_In_Rules()
    {
        IValidatorManager<Employee, Employee> validatorManager = new EmployeeValidatorWithBuildInRules();

        var propertyManager = (IPropertyValidatorManager<Employee, string>)validatorManager.PropertyValidators["FirstName"];

        Assert.Equal(5, propertyManager.Rules.Count);
    }

    [Fact]
    public void Should_Have_Seven_Rules()
    {
        IValidatorManager<Employee, Employee> validatorManager = new EmployeeValidatorWithSevenRulesInFirstNamePropValidator();

        var propertyManager = (IPropertyValidatorManager<Employee, string>)validatorManager.PropertyValidators["FirstName"];

        Assert.Equal(7, propertyManager.Rules.Count);
    }

    [Fact]
    public void Should_Throw_When_Duplicate_PredicateRule_Is_Added()
    {
        Assert.Throws<DuplicateRuleException>(() => new EmployeeValidatorWithDuplicatePredicateRule());
    }

    [Fact]
    public void Should_Throw_When_Nullable_Member_Is_Selected_In_PredicateRule()
    {
        Assert.Throws<NullableMemberException>(() => new EmployeeValidatorWithNullPredicateMembers());
    }

    [Fact]
    public void Should_Throw_When_Nullable_Member_Is_Selected_In_ComparisonRule()
    {
        Assert.Throws<NullableMemberException>(() => new EmployeeValidatorWithNullComparisonMembers());
    }
}

public sealed class EmployeeValidatorWithBuildInRules : AbstractValidator<Employee>
{
    public EmployeeValidatorWithBuildInRules()
    {
        ValidationsFor(x => x.FirstName)
            .EqualTo("test")
            .EqualTo(x => x.LastName)
            .NotEqualTo("test")
            .NotEqualTo(x => x.WorkInfo.Email)
            .NotEmpty();
    }
}

public sealed class EmployeeValidatorWithSevenRulesInFirstNamePropValidator : AbstractValidator<Employee>
{
    public EmployeeValidatorWithSevenRulesInFirstNamePropValidator()
    {
        ValidationsFor(x => x.FirstName)
            .FailsWhen(x => x.Length < 4 || x.Length > 7).WithErrorMessage("FistName must be more then 4 characters and less the 7")
            .FailsWhen(x => Char.IsLower(x[0])).WithErrorMessage("Fist letter of FirstName must be Uppercase.")
            .FailsWhen(x => string.IsNullOrEmpty(x)).WithErrorMessage("FirstName cant be empty.")
            .FailsWhen(x => x.Contains('@')).WithErrorMessage("FirstName cant contain @")
            .FailsWhen((x, y) => x.WorkInfo.Email == y).WithErrorMessage("FirstName cant be the some as Work Email")
            .FailsWhen((x, y) => x.FullName == y).WithErrorMessage("FirstName cant be some as FullName")
            .FailsWhen((x, y) => x.LastName == y).WithErrorMessage("FirstName cant be some as LastName");
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
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        ValidationsFor(x => x.PersonalInfo).FailsWhen(x => x.Address.City == "a");
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}

public sealed class EmployeeValidatorWithNullComparisonMembers : AbstractValidator<Employee>
{
    public EmployeeValidatorWithNullComparisonMembers()
    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        ValidationsFor(x => x.FirstName).FailsWhen((x, y) => x.PersonalInfo.Email == y);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
    }
}
