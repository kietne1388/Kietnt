using Microsoft.AspNetCore.Mvc;
using FastFood.Filters;
using FastFood.Models.Cart;
using System.Text.Json;

namespace FastFood.Areas.User.Controllers
{
    [Area("User")]
    [ServiceFilter(typeof(AuthFilter))]
    public class CartController : Controller
    {
        private const string CART_SESSION_KEY = "Cart";

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public IActionResult AddToCart(int productId, string productName, decimal price, int quantity = 1, string imageUrl = "")
        {
            var cart = GetCart();
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.Items.Add(new CartItemModel
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity,
                    ImageUrl = imageUrl
                });
            }

            SaveCart(cart);
            TempData["Success"] = "Product added to cart!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                if (quantity > 0)
                    item.Quantity = quantity;
                else
                    cart.Items.Remove(item);

                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (item != null)
            {
                cart.Items.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            var cart = GetCart();
            if (!cart.Items.Any())
            {
                return RedirectToAction("Index");
            }

            var model = new CheckoutModel { Items = cart.Items };
            return View(model);
        }

        private CartViewModel GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CART_SESSION_KEY);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new CartViewModel();
            }

            return JsonSerializer.Deserialize<CartViewModel>(cartJson) ?? new CartViewModel();
        }

        private void SaveCart(CartViewModel cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CART_SESSION_KEY, cartJson);
        }
    }
}
