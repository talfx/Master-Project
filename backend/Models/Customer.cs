namespace Backend.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? UserId { get; set; }

    // Navigation property
    public User? User { get; set; }
    // Navigation property
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}