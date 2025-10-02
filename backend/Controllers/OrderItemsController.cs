using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrderItemsController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public OrderItemsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetAllOrderItems()
    {
        var orderItems = await _context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .ToListAsync();
        return Ok(orderItems);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderItemById(int id)
    {
        var orderItem = await _context.OrderItems
            .Include(oi => oi.Order)
            .Include(oi => oi.Product)
            .FirstOrDefaultAsync(oi => oi.OrderItemId == id);
        
        if (orderItem == null)
        {
            return NotFound(new { message = $"Order item with ID {id} not found" });
        }
        
        return Ok(orderItem);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Employee,Customer")]
    public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemCreateRequest request)
    {
        var orderItem = new OrderItem
        {
            OrderId = request.OrderId,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            Subtotal = request.Quantity * request.UnitPrice
        };
        
        _context.OrderItems.Add(orderItem);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetOrderItemById), new { id = orderItem.OrderItemId }, orderItem);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> UpdateOrderItem(int id, [FromBody] OrderItemUpdateRequest request)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        
        if (orderItem == null)
        {
            return NotFound(new { message = $"Order item with ID {id} not found" });
        }
        
        orderItem.Quantity = request.Quantity;
        orderItem.UnitPrice = request.UnitPrice;
        orderItem.Subtotal = request.Quantity * request.UnitPrice;
        
        await _context.SaveChangesAsync();
        
        return Ok(orderItem);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> DeleteOrderItem(int id)
    {
        var orderItem = await _context.OrderItems.FindAsync(id);
        
        if (orderItem == null)
        {
            return NotFound(new { message = $"Order item with ID {id} not found" });
        }
        
        _context.OrderItems.Remove(orderItem);
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Order item deleted successfully" });
    }
}

public class OrderItemCreateRequest
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class OrderItemUpdateRequest
{
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}