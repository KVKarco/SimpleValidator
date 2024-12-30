using SimpleValidator.Internal.Builders;

namespace SimpleValidator.Tests;

public class AbstractValidatorTests
{
    private readonly IValidatorManager<Employee, Employee> _validator;
    private Employee _employee;

    public AbstractValidatorTests()
    {
        _validator = new EmployeeValidator();
        _employee = new()
        {
            Id = 1,
            FirstName = "testName",
            LastName = "testLastName",
            Age = 15,
            CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow),
            WorkInfo = new()
            {
                Id = 2,
                Email = "TestEmail",
                Phone = "1234567"
            }
        };
    }


    [Fact]
    public void Validator_Should_Have_Correct_ErrorMessages()
    {
        ValidationResult testResult = new();
        testResult.AddPropertyErrors("FirstName",
            "FistName must be more then 4 characters and less the 7",
            "Fist letter of FirstName must be Uppercase.");
        testResult.AddPropertyError("Age", "Minimum age for employee is 18.");
        testResult.AddPropertyErrors("CreatedAt", "CreatedAt date cant be in the future");

        var result = ((IValidator<Employee>)_validator).Validate(_employee);

        Assert.Equal(testResult.ValidationErrors, result.ValidationErrors);
    }
}
