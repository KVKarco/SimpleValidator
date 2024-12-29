namespace SimpleValidator.Tests;

public sealed class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        ValidationsFor(x => x.FirstName)
            .FailsWhen(x => x.Length < 4 || x.Length > 7).WithErrorMessage("FistName must be more then 4 characters and less the 7")
            .FailsWhen(x => Char.IsLower(x[0])).WithErrorMessage("Fist letter of FirstName must be Uppercase.")
            .FailsWhen(x => string.IsNullOrEmpty(x)).WithErrorMessage("FirstName cant be empty.");

        ValidationsFor(x => x.Age, NullOptions.FailsWhenNull)
            .FailsWhen(x => x < 18).WithErrorMessage("Minimum age for employee is 18.");

        ValidationsFor(x => x.CreatedAt)
            .FailsWhen(x => x > DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1))).WithErrorMessage("CreatedAt date cant be in the future");
    }
}
