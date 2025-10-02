using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;
    
    public ProductsController(AppDbContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        return Ok(product);
    }
    
    [HttpPost]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> CreateProduct([FromBody] ProductCreateRequest request)
    {
        var product = new Product
        {
            ProductName = request.ProductName,
            Description = request.Description,
            Category = request.Category,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetProductById), new { id = product.ProductId }, product);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateRequest request)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        product.ProductName = request.ProductName;
        product.Description = request.Description;
        product.Category = request.Category;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        
        await _context.SaveChangesAsync();
        
        return Ok(product);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        
        if (product == null)
        {
            return NotFound(new { message = $"Product with ID {id} not found" });
        }
        
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        
        return Ok(new { message = "Product deleted successfully" });
    }
}

public class ProductCreateRequest
{
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}

public class ProductUpdateRequest
{
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}