using System.ComponentModel.DataAnnotations;
using FTELMarket_Abidi.Infrastructure;
using FTELMarket_Abidi.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FTELMarket_Abidi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController(FtelMarketDbContext db) : ControllerBase
{
    public class ProductResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CategoryName { get; set; } = "";
    }

    public class ProductRequest
    {
        [Required, StringLength(200, MinimumLength = 1)]
        public string Name { get; set; } = "";

        [StringLength(1000)]
        public string? Description { get; set; }

        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
    }

    private static ProductResponse ToResponse(Product p, string categoryName) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Description = p.Description,
        Price = p.Price,
        Stock = p.Stock,
        CategoryId = p.CategoryId,
        CreatedAt = DateTime.SpecifyKind(p.CreatedAt, DateTimeKind.Utc),
        CategoryName = categoryName
    };

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var products = await db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .OrderBy(p => p.Id)
            .ToListAsync();

        return Ok(products.Select(p => ToResponse(p, p.Category.Name)));
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(ProductRequest request)
    {
        var category = await db.Categories.FindAsync(request.CategoryId);
        if (category is null)
            return BadRequest(new { error = "Catégorie inconnue." });

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId,
            CreatedAt = DateTime.UtcNow
        };
        db.Products.Add(product);
        await db.SaveChangesAsync();

        return Created($"api/products/{product.Id}", ToResponse(product, category.Name));
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponse>> Update(int id, ProductRequest request)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();

        var category = await db.Categories.FindAsync(request.CategoryId);
        if (category is null)
            return BadRequest(new { error = "Catégorie inconnue." });

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;
        await db.SaveChangesAsync();

        return Ok(ToResponse(product, category.Name));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await db.Products.FindAsync(id);
        if (product is null) return NotFound();

        db.Products.Remove(product);
        await db.SaveChangesAsync();
        return NoContent();
    }
}