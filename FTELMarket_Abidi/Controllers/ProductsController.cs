using Microsoft.AspNetCore.Mvc;

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

    private static readonly List<ProductResponse> Products = new()
    {
        new() { Id = 1, Name = "Gran Turismo 7", Description = "Boîte complète.", Price = 29.9m, Stock = 1, CategoryId = 1, CategoryName = "PlayStation 5", CreatedAt = DateTime.UtcNow },
        new() { Id = 2, Name = "The Legend of Zelda: Tears of the Kingdom", Description = "Cartouche seule.", Price = 39.9m, Stock = 2, CategoryId = 2, CategoryName = "Nintendo Switch", CreatedAt = DateTime.UtcNow }
    };

    [HttpGet]
    public ActionResult<IEnumerable<ProductResponse>> GetAll() => Ok(Products);
}