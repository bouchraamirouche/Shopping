using Microsoft.AspNetCore.Mvc;
using Shopping.Infrastructure;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class CartController : Controller
    {

        private readonly ShoppingContext context;

        public CartController(ShoppingContext context)
        {

            this.context = context;
        }
        //Get/cart/
        public IActionResult Index()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartViewModel cartVM = new CartViewModel
            { CartItems = cart,
                GrandTotal = cart.Sum(x => x.Price * x.Quantity)
            };
            
            return View(cartVM);
        } 
        
        //Get/cart/add/
        public async Task<IActionResult> Add(int id)
        {
            Product product = await context.Products.FindAsync(id);
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            CartItem cartItem=cart.Where(x=>x.ProductId==id).FirstOrDefault();
            if (cartItem == null)
            {
                cart.Add(new CartItem(product));

            }else
            {
                cartItem.Quantity += 1;

            }
            /*HttpContext.Session.SetJson("Cart", cart);
            return LocalRedirect("/Cart/index"); */
            HttpContext.Session.SetJson("Cart", cart);

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return LocalRedirect("/Cart/index");

            return ViewComponent("SmallCart");


        }
        public IActionResult Decrease(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            CartItem cartItem = cart.Where(x => x.ProductId == id).FirstOrDefault();

            if (cartItem.Quantity > 1)
            {
                --cartItem.Quantity;
            }
            else
            {
                cart.RemoveAll(x => x.ProductId == id);
            }

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return LocalRedirect("/Cart/index");
        }

        // GET /cart/remove/5
        public IActionResult Remove(int id)
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");

            cart.RemoveAll(x => x.ProductId == id);

            if (cart.Count == 0)
            {
                HttpContext.Session.Remove("Cart");
            }
            else
            {
                HttpContext.Session.SetJson("Cart", cart);
            }

            return LocalRedirect("/Cart/index");
        }

        // get/cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                return Redirect(Request.Headers["Referer"].ToString());

            return Ok();
        }

    }
}
