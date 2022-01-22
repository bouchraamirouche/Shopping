using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class PagesController : Controller
    {
        private readonly ShoppingContext context;
        public PagesController(ShoppingContext context)
        {
            this.context = context;
        }
        //GET/
        public async Task<IActionResult> Page(string slug)
        {
            if(slug == null) 
            {
                return View(await context.Pages.Where(x=>x.Slug == "home").FirstOrDefaultAsync());
            
            }
            Page page = await context.Pages.Where(x => x.Slug == slug).FirstOrDefaultAsync();
            if(page==null)
            {
                return NotFound();
            }
            return View(page);  
       
        }
    }
}
