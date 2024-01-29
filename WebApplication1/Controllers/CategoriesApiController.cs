using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesApiController : ControllerBase
    {
        private const int PageSize = 10;
        private readonly ArticleDbContext _context;

        public CategoriesApiController(ArticleDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery]int page = 1)
        {
            return _context.Categories != null ?
                        Ok(await _context.Categories
                            .OrderBy(x => x.Id)
                            .Skip((page - 1) * PageSize)
                            .Take(PageSize).ToListAsync()) :
                        NotFound("Entity set 'ArticleDbContext.Categories'  is null.");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetById(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<Category> Create([FromBody] Category category)
        {
            _context.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody]Category updatedCategory)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category is null) return NotFound();

            category.Name = updatedCategory.Name;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
