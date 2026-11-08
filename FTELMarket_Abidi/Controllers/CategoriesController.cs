using Microsoft.AspNetCore.Mvc;

namespace FTELMarket_Abidi.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoriesController : ControllerBase
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    [HttpGet]
    public ActionResult<IEnumerable<CategoryResponse>> GetAll() => Ok(new List<CategoryResponse>
    {
        new() { Id = 1, Name = "PlayStation 5" },
        new() { Id = 2, Name = "Nintendo Switch" },
        new() { Id = 3, Name = "Xbox Series X" }
    });
}