using Microsoft.AspNetCore.Mvc;
using MusicStore.Data;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly MusicStoreEntities _context;
        private ShoppingCart shoppingCart;
        public AccountController(MusicStoreEntities context)
        {
            _context = context;
            shoppingCart = new ShoppingCart();
            shoppingCart._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = shoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            string cartId=ShoppingCart.CartSessionKey;
            HttpContext.Session.SetString(cartId, UserName);
            //Session[ShoppingCart.CartSessionKey] = UserName;
        }


    }
}
