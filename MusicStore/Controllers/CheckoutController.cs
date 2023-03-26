using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicStore.Data;
using MusicStore.Models;

namespace MusicStore.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly MusicStoreEntities _context;
        const string PromoCode = "FREE";
        private ShoppingCart shoppingCart;

        public CheckoutController(MusicStoreEntities context)
        {
            _context = context;
            shoppingCart=new ShoppingCart();
            shoppingCart._context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddressAndPayment()
        {
            return View();
        }


        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(IFormCollection values)
        {
            var order = new Order();
            //TryUpdateModel(order);
            string ssss=values["Address"];
            try
            {               
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.FirstName = values["FirstName"];
                    order.LastName = values["LastName"];
                    order.OrderDate = DateTime.Now;
                    order.Address = values["Address"];
                    order.City = values["City"];
                    order.Country = values["Country"];
                    order.Email = values["Email"];
                    order.Phone = values["Phone"];
                    order.State = values["State"];
                    order.PostalCode = values["PostalCode"];
                    order.Total =Convert.ToDecimal(values["Total"]);
                    //Save Order
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    //Process the order
                    var cart = shoppingCart.GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    ViewData["CartCount"] = cart.GetCount();

                    return RedirectToAction("Complete",  new { id = order.OrderId });
                }
            }
            catch(Exception ex)
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }


        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = _context.Orders.Any( o => o.OrderId == id &&  o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
