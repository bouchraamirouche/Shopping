using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Shopping.Infrastructure;
using Shopping.Models;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Authorization;

namespace Shopping.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly ShoppingContext context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductsController(ShoppingContext context, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment=webHostEnvironment;
            this.context = context;
        }
        //get /admin/Products/
        public async Task<IActionResult> Index( int p=1)
        {
            int pageSize = 6;
            var products = context.Products
                .OrderByDescending(x => x.Id).Include(x => x.Category)
                .Skip((p - 1)*pageSize)
                .Take(pageSize);
            ViewBag.PageNumber = p;
            ViewBag.PageRange = pageSize;
            ViewBag.TotalPages = (int) Math.Ceiling((Decimal)context.Products.Count()/pageSize);
            return View(await products.ToListAsync());
        }
        //get /admin/products/create /5
        public IActionResult Create() {

            /*  ViewBag.CategoryId = new List<SelectListItem>
      {
          new SelectListItem {Text = "fruit", Value = "1"},
          new SelectListItem {Text = "batata", Value = "2"}
      };*/
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name");

            return View();
        }


        //Post /admin/products/creates/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                TempData["Success"] = "product added Successfully";
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["Error"] = "product added Failed ! <br/>Please fill the required Fields.";
                //return RedirectToAction("Index");
            }
            product.Slug = product.Name.ToLower().Replace(" ", "-");
            var slug = await context.Products.FirstOrDefaultAsync(X => X.Slug == product.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "this product already exists");
                return View(product);
            }
            String imageName = "noimage.png";
            if (product.ImageUpload != null)
            {
                string contentRootPath = _webHostEnvironment.ContentRootPath;
                string webRootPath = _webHostEnvironment.WebRootPath;
                //webHostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                String uploadDir = Path.Combine(webRootPath, "media/products");
                imageName = Guid.NewGuid().ToString()+"_"+ product.ImageUpload.FileName;
                string filePath = Path.Combine(uploadDir, imageName);
                FileStream fs = new FileStream(filePath,FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
            }
            product.Image = imageName;

            context.Add(product);
            await context.SaveChangesAsync();
            TempData["Success"] = "product added Successfully";

            return RedirectToAction("Index");
            //return View(page);

        }
        //GET  /admin/products/details/id
        public async Task<IActionResult> Details(int id)
        {
            Product product = await context.Products.Include(x=>x.Category).FirstOrDefaultAsync(x=>x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }


        //GET  /admin/products/edit/id
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name",product.CategoryId);

            return View(product);

        }


        //Post /admin/products/edit/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Product product)
        {
            ViewBag.CategoryId = new SelectList(context.Categories.OrderBy(x => x.Sorting), "Id", "Name", product.CategoryId);

            if (ModelState.IsValid)
            {
                TempData["Success"] = "product adited Successfully";
                //return RedirectToAction("Index", "Home");
            }
            else
            {
                //TempData["Error"] = "product added Failed ! <br/>Please fill the required Fields.";
                //return RedirectToAction("Index");
            }
            product.Slug = product.Name.ToLower().Replace(" ", "-");
            var slug = await context.Products.Where(x =>x.Id != id).FirstOrDefaultAsync(X => X.Slug == product.Slug);
            if (slug != null)
            {
                ModelState.AddModelError("", "this edited already exists");
                return View(product);
            }
            
            if (product.ImageUpload != null)
            {
                string contentRootPath = _webHostEnvironment.ContentRootPath;
                string webRootPath = _webHostEnvironment.WebRootPath;
                //webHostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                String uploadDir = Path.Combine(webRootPath, "media/products");
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    String oldImagePath = Path.Combine(uploadDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                }
                 string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpload.FileName;

                string filePath = Path.Combine(uploadDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                await product.ImageUpload.CopyToAsync(fs);
                fs.Close();
                product.Image = imageName;
            }
           

            context.Update(product);
            await context.SaveChangesAsync();
            TempData["Success"] = "product has been editesSuccessfully";

            return RedirectToAction("Index");
            //return View(page);

        }

        //GET  /admin/prod/delete/id
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await context.Products.FindAsync(id);
            if (product == null)
            {
                TempData["Error"] = "the product does not exist";
            }
            else
            {
                if (!string.Equals(product.Image, "noimage.png"))
                {
                    string webRootPath = _webHostEnvironment.WebRootPath;
                    String uploadDir = Path.Combine(webRootPath, "media/products");

                    String oldImagePath = Path.Combine(uploadDir, product.Image);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                TempData["Success"] = "product deleted Successfully";

            }
            return (RedirectToAction("Index"));

        }


    }
}
