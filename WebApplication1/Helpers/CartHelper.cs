using WebApplication1.Data;
using WebApplication1.Models;
using System.Text.Json;

namespace WebApplication1.Helpers;

public class CartHelper
{
    private readonly ArticleDbContext _context;

    public CartHelper(ArticleDbContext context)
    {
        _context = context;
    }

    public List<CartItem> GetCart(string cartJson)
    {
        var cart = string.IsNullOrEmpty(cartJson) ? new Dictionary<int, int>() :
               JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson);

        return GetCartItems(cart);
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