using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FTELMarket.Data;
namespace FTELMarket.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly FTELMarketDbContext _context;

    // Injection du DbContext configuré avec PostgreSQL
    public CategoriesController(FTELMarketDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
    {
        var categories = await _context.Categories
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToListAsync();

        return Ok(categories);
    }
}

public class CategoryDto
{
    public int Id { get; set;}
    public required string Name { get; set;}
}