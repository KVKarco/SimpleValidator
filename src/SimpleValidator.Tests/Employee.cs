namespace SimpleValidator.Tests;

public class Employee
{
    public int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public int? Age { get; set; }

    public DateOnly CreatedAt { get; set; }

    public required Info WorkInfo { get; set; }

    public Info? PersonalInfo { get; set; }

    public string FullName => FirstName + " " + LastName;
}

public class Info
{
    public int Id { get; set; }

    public required string Email { get; set; }

    public string? Phone { get; set; }

    public Address? Address { get; set; }
}

public class Address
{
    public required string Country { get; set; }
    public required string Street { get; set; }
    public required string City { get; set; }
    public int PostalCode { get; set; }
    public int StreetNumber { get; set; }
}