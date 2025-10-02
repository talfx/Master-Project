using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public OrdersController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var orders = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .ToListAsync();
        return Ok(orders);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var order = await _context.Orders
            .Include(o => o.Customer)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);
        
        if (order == null)
        {
            return NotFound(new { message = $"Order with ID {id} not found" });
        }
        
        return Ok(order);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Employee,Customer")]
    public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequest request)
    {
        var order = new Order
        {
            CustomerId = request.CustomerId,
            OrderDate = DateTime.UtcNow,
            Status = "pending",
            TotalAmount = request.TotalAmount,
            ShippingAddress = request.ShippingAddress,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Orders.Add(order);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderUpdateRequest request)
    {
        var order = await _context.Orders.FindAsync(id);
        
        if (order == null)
        {
            return NotFound(new { message = $"Order with ID {id} not found" });
        }
        
        order.Status = request.Status;
        order.TotalAmount = request.TotalAmount;
        order.ShippingAddress = request.ShippingAddress;
        
        await _context.SaveChangesAsync();
        
        return Ok(order);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        
        if (order == null)
        {
            return NotFound(new { message = $"Order with ID {id} not found" });
        }
        
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Order deleted successfully" });
    }
}

public class OrderCreateRequest
{
    public int CustomerId { get; set; }
    public decimal? TotalAmount { get; set; }
    public string? ShippingAddress { get; set; }
}

public class OrderUpdateRequest
{
    public string Status { get; set; } = string.Empty;
    public decimal? TotalAmount { get; set; }
    public string? ShippingAddress { get; set; }
}