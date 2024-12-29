using SimpleValidator.Exceptions;
using SimpleValidator.Internal.Builders;

namespace SimpleValidator.Tests;

public class PropertyValidatorsCreationTests
{
    [Fact]
    public void Should_Have_Four_PropertyValidators()
    {
        IValidatorManager<Employee, Employee> validatorManager = new EmployeeValidatorWithFourPropertyValidators();

        Assert.Equal(4, validatorManager.PropertyValidators.Count);
    }

    [Fact]
    public void Should_Throw_When_Nested_Property_Is_Selected()
    {
        Assert.Throws<InvalidSelectorException>(() => new EmployeeValidatorWithNestedSelection());
    }
}

public sealed class EmployeeValidatorWithNestedSelection : AbstractValidator<Employee>
{
    public EmployeeValidatorWithNestedSelection()
    {
        ValidationsFor(x => x.PersonalInfo.Email);
    }
}

public sealed class EmployeeValidatorWithFourPropertyValidators : AbstractValidator<Employee>
{
    public EmployeeValidatorWithFourPropertyValidators()
    {
        ValidationsFor(x => x.FirstName)
            .FailsWhen(x => x.Length < 4 || x.Length > 7).WithErrorMessage("FistName must be more then 4 characters and less the 7")
            .FailsWhen(x => Char.IsLower(x[0])).WithErrorMessage("Fist letter of FirstName must be Uppercase.")
            .FailsWhen(x => string.IsNullOrEmpty(x)).WithErrorMessage("FirstName cant be empty.");

        ValidationsFor(x => x.Age, NullOptions.FailsWhenNull)
            .FailsWhen(x => x < 18).WithErrorMessage("Minimum age for employee is 18.");

        ValidationsFor(x => x.CreatedAt)
            .FailsWhen(x => x > DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))).WithErrorMessage("CreatedAt date cant be in the future");

        ValidationsFor(x => x.LastName, NullOptions.NotValidateWhenNull)
            .FailsWhen(x => x.Length == 0).WithErrorMessage("LastName cant be empty")
            .FailsWhen(x => x.Length < 4 || x.Length > 7).WithErrorMessage("FistName must be more then 4 characters and less the 7");
    }
}