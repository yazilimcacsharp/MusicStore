using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicStore.Data;
using MusicStore.Models;
using MusicStore.ViewModels;
using System.Text.Encodings.Web;

namespace MusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly MusicStoreEntities _context;
        private ShoppingCart shoppingCart;
        public ShoppingCartController(MusicStoreEntities context)
        {
            _context = context;
            shoppingCart = new ShoppingCart();
            shoppingCart._context = context;
        }


        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {          
            //shoppingCart._context = _context;
            var cart = shoppingCart.GetCart(this.HttpContext);
           
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            ViewData["CartCount"] = HttpContext.Session.GetString("adet");
            return View(viewModel);
        }


        //
        // GET: /Store/AddToCart/5
        public IActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedAlbum = _context.Albums.Single(album => album.AlbumId == id);

            // Add it to the shopping cart
            var cart = shoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedAlbum);
            HttpContext.Session.SetString("adet",cart.GetCount().ToString());
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }

        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public IActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = shoppingCart.GetCart(this.HttpContext);

            // Get the name of the album to display confirmation
            string albumName = _context.Carts.Include(p=>p.Album).Single(item => item.RecordId == id).Album.Title;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            HttpContext.Session.SetString("adet", cart.GetCount().ToString());

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = HtmlEncoder.Default.Encode(albumName) +" has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }


        //
        // GET: /ShoppingCart/CartSummary     
        public IActionResult CartSummary()
        {
            var cart = shoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();            
            return PartialView("CartSummary");
        }

   
    }
}
