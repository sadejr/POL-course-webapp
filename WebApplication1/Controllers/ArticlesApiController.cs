using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticlesApiController : ControllerBase
    {
        private const int PageSize = 5;
        private readonly ArticleDbContext _context;

        public ArticlesApiController(ArticleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetAll([FromQuery] int page = 1)
        {
            return _context.Article != null ?
                Ok(await _context.Article
                    .Include(a => a.Category)
                    .OrderBy(x => x.Id)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize).ToListAsync()) :
                NotFound("Entity set 'ArticleDbContext.Articles' is null.");

        }

        [HttpPost]
        public async Task<Article> Create([FromBody] Article article)
        {
            _context.Add(article);
            await _context.SaveChangesAsync();

            return article;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetById(int id)
        {
            var article = await _context.Article.FindAsync(id);
            if (article is null) return NotFound();

            return Ok(article);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Article updatedArticle)
        {
            var article = await _context.Article.FindAsync(id);
            if (article is null) return NotFound();

            article.Name = updatedArticle.Name;
            article.Price = updatedArticle.Price;
            article.CategoryId = updatedArticle.CategoryId;
            _context.Article.Update(article);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            var article = await _context.Article.FindAsync(id);

            if (article != null)
            {
                _context.Article.Remove(article);
            }

            await _context.SaveChangesAsync();
        }
    }
}
