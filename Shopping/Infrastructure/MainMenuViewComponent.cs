using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;

namespace Shopping.Infrastructure
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly ShoppingContext context;
        public MainMenuViewComponent(ShoppingContext context)
        {
            this.context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var pages = await GetPageAsync();
            return View(pages); 
        }
        private Task<List<Page>> GetPageAsync()
        {
            return context.Pages.OrderBy(x => x.Sorting).ToListAsync();
        }
    }
}
