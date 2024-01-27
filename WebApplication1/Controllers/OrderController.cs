using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = RoleNames.Customer)]
    public class OrderController : Controller
    {
        private readonly CartHelper _cartHelper;

        public OrderController(CartHelper cartHelper)
        {
            _cartHelper = cartHelper;
        }

        public IActionResult Index()
        {
            var cartItems = _cartHelper.GetCart(Request.Cookies["Cart"]);

            if (cartItems is null || cartItems.Count <= 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var order = new Order { CartItems = cartItems };
            return View(order);
        }

        [HttpPost]
        public IActionResult Confirm(Order order)
        {
            ClearCart();
            return View(order);
        }

        private void ClearCart()
        {
            string cartJson = JsonSerializer.Serialize(new Dictionary<int, int>());
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append("Cart", cartJson, options);
        }
    }
}
