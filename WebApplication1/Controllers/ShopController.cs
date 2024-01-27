using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Text.Json; // Needed for LINQ queries

namespace WebApplication1.Controllers
{
    public class ShopController : Controller
    {
        private readonly ArticleDbContext _context;

        public ShopController(ArticleDbContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var context = _context.Article.Include(a => a.Category);

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");

            return View(await context.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int? category)
        {
            var context = _context.Article.Include(a => a.Category);

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");

            var arts = await context.ToListAsync();

            if (category.HasValue && category.Value > 0)
            {
                arts = arts.Where(art => art.CategoryId == category.Value).ToList();
            }

            return View(arts);
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            SaveCartToCookies(new Dictionary<int, int>());

            return RedirectToAction("Cart");
        }

        public IActionResult AddToCart(int productId)
        {
            var cart = GetCartFromCookies();
            if (cart.ContainsKey(productId))
            {
                cart[productId] += 1;
            }
            else
            {
                cart[productId] = 1;
            }
            SaveCartToCookies(cart);
            return RedirectToAction("Index");
        }

        public IActionResult Cart()
        {
            var cart = GetCartFromCookies();
            var cartItems = GetCartItems(cart);
            return View(cartItems);
        }

        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCartFromCookies();
            if (cart.ContainsKey(productId))
            {
                cart.Remove(productId);
                SaveCartToCookies(cart);
            }
            return RedirectToAction("Cart");
        }

        public IActionResult UpdateCart(int productId, int quantity)
        {
            var cart = GetCartFromCookies();
            if (cart.ContainsKey(productId) && quantity > 0)
            {
                cart[productId] = quantity;
            }
            else if (quantity == 0)
            {
                cart.Remove(productId);
            }
            SaveCartToCookies(cart);
            return RedirectToAction("Cart");
        }

        private Dictionary<int, int> GetCartFromCookies()
        {
            string cartJson = Request.Cookies["Cart"];
            return string.IsNullOrEmpty(cartJson) ? new Dictionary<int, int>() :
                   JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson);
        }

        private void SaveCartToCookies(Dictionary<int, int> cart)
        {
            string cartJson = JsonSerializer.Serialize(cart);
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append("Cart", cartJson, options);
        }

        private List<CartItem> GetCartItems(Dictionary<int, int> cart)
        {
            var cartItems = new List<CartItem>();
            foreach (var entry in cart)
            {
                var article = _context.Article.FirstOrDefault(a => a.Id == entry.Key);
                if (article != null)
                {
                    cartItems.Add(new CartItem
                    {
                        ProductId = article.Id,
                        Name = article.Name,
                        Price = article.Price,
                        Quantity = entry.Value,
                        Total = entry.Value * article.Price
                    });
                }
            }
            return cartItems;
        }
    }
}
