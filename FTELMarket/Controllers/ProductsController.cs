using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FTELMarket.Data;
using FTELMarket.Data.Entities;

namespace FTELMarket.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly FTELMarketDbContext _context;

    public ProductsController(FTELMarketDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
    {
            var products = await _context.Products
            .Include(p => p.Category) // Récupère la catégorie liée
            .ToListAsync();

        var dtos = products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            CategoryId = p.CategoryId,
            CreatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            CategoryName = p.Category?.Name ?? string.Empty
        });

        return Ok(dtos);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> Post([FromBody]  ProductDto dto)
    {
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == dto.CategoryId);
        if (!categoryExists)
        {
            return BadRequest("La catégorie spécifiée n'existe pas.");
        }

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock,
            CategoryId = dto.CategoryId,
            UserId = 1, // Temporaire (en attendant l'authentification des utilisateurs)
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        await _context.Entry(product).Reference(p => p.Category).LoadAsync();

        var resultDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            CategoryName = product.Category?.Name ?? string.Empty
        };

        return CreatedAtAction(nameof(GetById), new { id = product.Id }, resultDto);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var dto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            CategoryName = product.Category?.Name ?? string.Empty
        };

        return Ok(dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> Put(int id, [FromBody] ProductDto dto)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return NotFound();
        }

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}


public class ProductDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
}
