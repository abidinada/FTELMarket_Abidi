using FTELMarket_Abidi.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FTELMarket_Abidi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController(FtelMarketDbContext db) : ControllerBase
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetAll() =>
        Ok(await db.Categories
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new CategoryResponse { Id = c.Id, Name = c.Name })
            .ToListAsync());
}