using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FTELMarket_Abidi.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
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

    private static readonly Dictionary<int, string> Categories = new()
    {
        [1] = "PlayStation 5",
        [2] = "Nintendo Switch",
        [3] = "Xbox Series X"
    };

    private static readonly List<ProductResponse> Products = new()
    {
        new() { Id = 1, Name = "Gran Turismo 7", Description = "Boîte complète.", Price = 29.9m, Stock = 1, CategoryId = 1, CategoryName = "PlayStation 5", CreatedAt = DateTime.UtcNow },
        new() { Id = 2, Name = "The Legend of Zelda: Tears of the Kingdom", Description = "Cartouche seule.", Price = 39.9m, Stock = 2, CategoryId = 2, CategoryName = "Nintendo Switch", CreatedAt = DateTime.UtcNow }
    };

    private static int _nextId = 3;

    [HttpGet]
    public ActionResult<IEnumerable<ProductResponse>> GetAll() => Ok(Products);

    [HttpPost]
    public ActionResult<ProductResponse> Create(ProductRequest request)
    {
        if (!Categories.TryGetValue(request.CategoryId, out var categoryName))
            return BadRequest(new { error = "Catégorie inconnue." });

        var product = new ProductResponse
        {
            Id = _nextId++,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            CategoryId = request.CategoryId,
            CategoryName = categoryName,
            CreatedAt = DateTime.UtcNow
        };
        Products.Add(product);
        return Created($"api/products/{product.Id}", product);
    }

    [HttpPut("{id}")]
    public ActionResult<ProductResponse> Update(int id, ProductRequest request)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product is null) return NotFound();
        if (!Categories.TryGetValue(request.CategoryId, out var categoryName))
            return BadRequest(new { error = "Catégorie inconnue." });

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.Stock = request.Stock;
        product.CategoryId = request.CategoryId;
        product.CategoryName = categoryName;
        return Ok(product);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var product = Products.FirstOrDefault(p => p.Id == id);
        if (product is null) return NotFound();
        Products.Remove(product);
        return NoContent();
    }
}