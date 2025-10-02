namespace Backend.Models;

public class Employee
{
    public int EmployeeId { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Department { get; set; }
    public string? Position { get; set; }
    public DateTime HireDate { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Navigation property
    public User User { get; set; } = null!;
}