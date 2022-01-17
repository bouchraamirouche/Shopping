using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure;
using Shopping.Models;

namespace Shopping.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
       

        private readonly ShoppingContext context;
        public CategoriesController(ShoppingContext context)
        {
            this.context = context;
        }
        //get /admin/Categories/
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.OrderBy(x =>x.Sorting).ToListAsync());
        }
        //get /admin/Categories/create /5
        public IActionResult Create() => View();

        //Post /admin/Categories/creates/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                TempData["Success"] = "category added Successfully";
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["Error"] = "category added Failed ! <br/>Please fill the required Fields.";
                //return RedirectToAction("Index");
                //return View(category);///////??
            }
            category.Slug = category.Name.ToLower().Replace(" ", "-");
            category.Sorting = 100;
            var slug = await context.Pages.FirstOrDefaultAsync(X => X.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "this name already exists");
                return View(category);
            }
            context.Add(category);
            await context.SaveChangesAsync();
            TempData["Success"] = "category added Successfully";

            return RedirectToAction("Index");
            //return View(category);

        }
        //GET  /admin/category/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);

        }
        //Post /admin/categories/EDIT/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Category category)
        {
            if (ModelState.IsValid)
            {
                TempData["Success"] = "pages added Successfully";
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                // TempData["Error"] = "pages added Failed ! Please fill the required Fields.";
                //return RedirectToAction("Index");
            }
            category.Slug =  category.Name.ToLower().Replace(" ", "-");
            
            var slug = await context.Categories.Where(x => x.Id != id).FirstOrDefaultAsync(X => X.Slug == category.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "this category already exists");
                return View(category);
            }
            context.Update(category);
            await context.SaveChangesAsync();
            TempData["Success"] = "category edites Successfully";

            return RedirectToAction("Edit", new { id });
            //return View(page);

        }

        //GET  /admin/category/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            Category category = await context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Error"] = "the category does not exist";
            }
            else
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                TempData["Success"] = "deleted category Successfully";

            }
            return (RedirectToAction("Index"));

        }

        //Post /admin/categories/reorder
        [HttpPost]

        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;
            //var pageId;

            foreach (var categoryId in id)
            {

                Category category;
                category = await context.Categories.FindAsync(categoryId);
                category.Sorting = count;
                context.Update(category);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }

    }
}
