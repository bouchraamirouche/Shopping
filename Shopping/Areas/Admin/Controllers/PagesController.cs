using Microsoft.AspNetCore.Mvc;

using Shopping.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Shopping.Models;
using Microsoft.AspNetCore.Authorization;

namespace Shopping.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin,editor")]
    [Area("Admin")]
    public class PagesController : Controller
    {
        private readonly ShoppingContext context;
        public PagesController(ShoppingContext context)
        {
            this.context = context;
        }
        //GET  /admin/pages
        public async Task<IActionResult > Index()
        {
            IQueryable < Page > pages= from p in context.Pages orderby p.Sorting select p;
            List<Page> pagesList = await pages.ToListAsync();
            return View("Index",pagesList);

        } 
        //GET  /admin/pages/details/id
        public async Task<IActionResult > Details(int id)
        {
           Page page = await context.Pages.FirstOrDefaultAsync(x=>x.Id==id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);

        }
        //get /admin/pages/create /5
        public IActionResult  Create ()=> View();

        //Post /admin/pages/creates/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult > Create(Page page)
        {
            if (ModelState.IsValid)
            {
                TempData["Success"] = "pages added Successfully";
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["Error"] = "pages added Failed ! <br/>Please fill the required Fields.";
                //return RedirectToAction("Index");
            }
            page.Slug = page.Title.ToLower().Replace(" ", "-");
            page.Sorting = 100;
            var slug = await context.Pages.FirstOrDefaultAsync(X => X.Slug == page.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "this title already exists");
                return View(page);
            }
            context.Add(page);
            await context.SaveChangesAsync();
            TempData["Success"] = "pages added Successfully";

            return RedirectToAction("Index");
            //return View(page);

        }
        //GET  /admin/pages/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                return NotFound();
            }
            return View(page);

        }
        //Post /admin/pages/EDIT/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Page page)
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
            page.Slug = page.Id==1 ? "home" :page.Title.ToLower().Replace(" ", "-");
            page.Slug = page.Title.ToLower().Replace(" ", "-");
            
            var slug = await context.Pages.Where(x=> x.Id !=page.Id).FirstOrDefaultAsync(X => X.Slug == page.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "this title already exists");
                return View(page);
            }
            context.Update(page);
            await context.SaveChangesAsync();
            TempData["Success"] = "pages editesSuccessfully";

            return RedirectToAction("Edit", new {id = page.Id});
            //return View(page);

        }
        //GET  /admin/pages/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            Page page = await context.Pages.FindAsync(id);
            if (page == null)
            {
                TempData["Error"] = "the pages does not exist";
            }
            else
            {
                context.Pages.Remove(page);
                await context.SaveChangesAsync();
                TempData["Success"] = "pages deleted Successfully";

            }
             return(RedirectToAction("Index"));

        }
        //Post /admin/pages/reorder
        [HttpPost]
       
        public async Task<IActionResult> Reorder(int[] id)
        {
            int count = 1;
            //var pageId;

            foreach ( var pageId in id)
            {
                
                Page page;
                page= await context.Pages.FindAsync(pageId);
                page.Sorting = count;
                context.Update(page);
                await context.SaveChangesAsync();
                count++;
            }
            return Ok();
        }


    }
}
